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

        public IEnumerable<Project> GetAll()
        {
            return _projectsRepository.GetAll();
        }

        public IEnumerable<Project> GetPage(Int32 pageNumber, Int32 pageSize)
        {
            IEnumerable<Project> projects = _projectsRepository.GetPage(pageNumber, pageSize);

            return projects;
        }

        public async Task<ExistingProjectWithTasks> GetProjectByName(String name, Int32 pageNumber, Int32 pageSize)
        {
            ExistingProjectWithTasks project = await _projectsRepository.GetProjectByName(name) as ExistingProjectWithTasks;
            project.ID = project.RowKey;
            IEnumerable<ProjectTaskEntity> tasks = await _tasksRepository.GetTasksForProject(name);

            IEnumerable<ProjectTask> newTasks = tasks.Select(task =>
            {
                return ProjectTask.CreateFromEntity(task);
            }); 

            project.ProjectTasks = newTasks.Skip(pageNumber * pageSize).Take(pageSize).ToList();
            project.NumberOfTasks = tasks.Count();
            return project;
        }

        public Int32 NumberOfProjects()
        {
            return GetAll().Count();
        }

        public async Task SaveNewProject(Project newProject)
        {
            await _projectsRepository.Save(newProject);
        }

        public async Task SaveNewProjectWithTasks(NewProjectWithTasks newProject)
        {
            await SaveNewProject(newProject);
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

            await _tasksRepository.SaveAll(tasks);
        }

        public async Task SaveNewTask(String projectName, String taskName)
        {
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
            Project projectToUpdate = new Project
            {
                RowKey = project.ID,
                ProjectName = project.ProjectName
            };

            await _projectsRepository.Update(projectToUpdate);

            foreach(ProjectTask task in project.ProjectTasks)
            {
                task.PartitionKey = project.ID;
                task.RowKey = task.ID;
            }

            await _tasksRepository.UpdateAll(project.ProjectTasks);
        }
    }
}