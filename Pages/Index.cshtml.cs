namespace ProjectsToDoList.Pages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using ProjectsToDoList.Interfaces;
    using ProjectsToDoList.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IndexModel> _logger;
        private readonly IProjectsService _projectsService;

        private Int32 _currentPage = 0;
        private readonly Int32 _pageSize = 20;
        
        public Int32 PageSize => _pageSize;
        public IEnumerable<Project> Projects { get; set; }

        public IndexModel(IConfiguration configuration, 
                            ILogger<IndexModel> logger,
                            IProjectsService projectsService)
        {
            Projects = new List<Project>();
            _configuration = configuration;
            _logger = logger;
            _projectsService = projectsService;
        }

        public void OnGet()
        {
            Projects = _projectsService.GetPage(_currentPage, _pageSize);
        }

        public async Task OnPostSaveNewProjectAsync(String projectName)
        {
            Project newProject = new Project
            {
                ProjectName = projectName
            };

            await _projectsService.SaveNewProject(newProject);
        }
    }
}
