namespace ProjectsToDoList.Services
{
    using Microsoft.Extensions.Logging;
    using ProjectsToDoList.Interfaces;
    using ProjectsToDoList.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProjectsService : IProjectsService
    {
        private readonly ILogger _logger;
        private readonly IProjectsRepository _projectsRepository;
        private readonly ITasksRepository _tasksRepository;

        public ProjectsService(ILogger<ProjectsService> logger, IProjectsRepository projectsRepository,
                                ITasksRepository tasksRepository)
        {
            _logger = logger;
            _projectsRepository = projectsRepository;
            _tasksRepository = tasksRepository;
        }

        public async Task DeleteProject(String projectID)
        {
            _logger.LogInformation($"Deleting project with ID of {projectID}");

            Boolean success = await _projectsRepository.DeleteAsync(projectID);
            if(success)
            {
                _logger.LogInformation($"{projectID} deleted successfully, deleting all tasks");
                await _tasksRepository.DeleteAllTasksForProjectAsync(projectID);
                _logger.LogInformation($"All Tasks for {projectID} deleted");
            }
        }

        public async Task DeleteTask(String taskID, String projectName)
        {
            _logger.LogInformation($"Deleting Task with ID {taskID} for {projectName}");
            await _tasksRepository.Delete(taskID, projectName);
        }

        public async Task<IEnumerable<Project>> GetAll()
        {
            _logger.LogDebug("Getting all Projects.");
            IEnumerable<Project> projects = await _projectsRepository.GetAll();

            foreach(Project project in projects)
            {
                project.Complete = await _tasksRepository.AllTasksAreCompleted(project.ProjectName);
            }

            return projects;
        }

        public async Task<IEnumerable<Project>> GetPage(Int32 pageNumber, Int32 pageSize, Boolean anonymise = false)
        {
            _logger.LogInformation($"Getting {pageSize} projects for Page No. {pageNumber}");
            IEnumerable<Project> projects = await _projectsRepository.GetPage(pageNumber, pageSize);

            if(anonymise)
            {
                projects.Where(project => project.Private).ToList().ForEach(project =>
                {
                    project.ProjectName = "Private Project";
                });
            }

            projects.ToList().ForEach(async project =>
            {
                project.Complete = await _tasksRepository.AllTasksAreCompleted(project.ProjectName);
            });

            return projects;
        }

        public async Task<ExistingProjectWithTasks> GetProjectByName(String name, Int32 pageNumber, Int32 pageSize)
        {
            _logger.LogInformation($"Getting Project: {name}");
            ExistingProjectWithTasks project = await _projectsRepository.GetProjectByName(name) as ExistingProjectWithTasks;
            project.ID = project.RowKey;

            _logger.LogInformation($"Getting Tasks for Project: {name}");
            IEnumerable<ProjectTaskEntity> tasks = await _tasksRepository.GetTasksForProject(name);

            IEnumerable<ProjectTask> newTasks = tasks.Select(task =>
            {
                return ProjectTask.CreateFromEntity(task);
            }); 

            project.ProjectTasks = newTasks.Skip(pageNumber * pageSize).Take(pageSize).ToList();
            project.NumberOfTasks = tasks.Count();
            return project;
        }

        public async Task<Int32> NumberOfProjects()
        {
            return (await GetAll()).Count();
        }

        public async Task SaveNewProject(Project newProject)
        {
            _logger.LogInformation($"Saving Project: {newProject.ProjectName}");
            await _projectsRepository.Save(newProject);
        }

        public async Task SaveNewProjectWithTasks(NewProjectWithTasks newProject)
        {
            _logger.LogInformation($"Saving new projects with Tasks, Project Name : {newProject.ProjectName}");
            await SaveNewProject(newProject);

            if(newProject.ProjectTasks != null && newProject.ProjectTasks.Any() )
            {
                List<ProjectTaskEntity> tasks = new List<ProjectTaskEntity>();
                foreach(String task in newProject.ProjectTasks)
                {
                    tasks.Add(new ProjectTaskEntity
                    {
                        PartitionKey = newProject.ProjectName,
                        RowKey = task,
                        TaskName = task
                    });       
                }

                _logger.LogInformation($"Saving {tasks.Count} tasks for {newProject.ProjectName}");
                await _tasksRepository.SaveAll(tasks);
            }
        }

        public async Task SaveNewTask(String projectName, String taskName)
        {
            _logger.LogInformation($"Saving Task with name of : {taskName} for {projectName}");

            ProjectTaskEntity newTask = new ProjectTaskEntity
            {
                PartitionKey = projectName,
                RowKey = taskName,
                TaskName = taskName
            };

            await _tasksRepository.SaveNewTask(newTask);
        }

        public async Task UpdateCurrentProject(ExistingProjectWithTasks project)
        {
            _logger.LogInformation($"Updating Project : {project.ProjectName}");
            Project projectToUpdate = new Project
            {
                Private = project.Private,
                RowKey = project.ID,
                ProjectName = project.ProjectName
            };

            await _projectsRepository.Update(projectToUpdate);

            _logger.LogInformation($"Updating all Tasks for {project.ProjectName}");
            foreach(ProjectTask task in project.ProjectTasks)
            {
                task.PartitionKey = project.ID;
                task.RowKey = task.ID;
            }

            await _tasksRepository.UpdateAll(project.ProjectTasks);
        }
    }
}