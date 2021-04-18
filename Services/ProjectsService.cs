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

        public ProjectsService(ILogger<ProjectsService> logger, IProjectsRepository projectsRepository)
        {
            _logger = logger;
            _projectsRepository = projectsRepository;
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

        public async Task SaveNewProject(Project newProject)
        {
            await _projectsRepository.Save(newProject);
        }
    }
}