namespace ProjectsToDoList.Services
{
    using Microsoft.Extensions.Logging;
    using ProjectsToDoList.Interfaces;
    using ProjectsToDoList.Models;
    using System;
    using System.Collections.Generic;
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

        public async Task<ExistingProjectWithTasks> GetProjectByName(String name)
        {
            ExistingProjectWithTasks project = await _projectsRepository.GetProjectByName(name) as ExistingProjectWithTasks;
            project.ProjectTasks = await _tasksRepository.GetTasksForProject(name);
            return project;
        }

        public async Task SaveNewProject(Project newProject)
        {
            await _projectsRepository.Save(newProject);
        }

        public async Task SaveNewProjectWithTasks(NewProjectWithTasks newProject)
        {
            await SaveNewProject(newProject);
            List<ProjectTask> tasks = new List<ProjectTask>();

            foreach(String task in newProject.ProjectTasks)
            {
                tasks.Add(new ProjectTask
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
            ProjectTask newTask = new ProjectTask
            {
                PartitionKey = projectName,
                RowKey = taskName,
                TaskName = taskName
            };

            await _tasksRepository.SaveNewTask(newTask);
        }
    }
}