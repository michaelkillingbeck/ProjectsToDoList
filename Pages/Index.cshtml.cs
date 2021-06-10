namespace ProjectsToDoList.Pages
{
    using Microsoft.AspNetCore.Mvc;
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
        private readonly Int32 _pageSize = 18;
        private Byte _totalPages = 0;
        
        public Int32 CurrentPage => _currentPage;
        public Boolean NextPageAvailable => _currentPage < _totalPages;
        public Int32 PageSize => _pageSize;
        public Boolean PreviousPageAvailable => _currentPage > 0;
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

        public IActionResult OnGet(Int32 pagenumber = 0)
        {
            _logger.LogDebug($"Getting projects for page {pagenumber}");

            _currentPage = pagenumber;
            Projects = _projectsService.GetPage(_currentPage, _pageSize);
            _totalPages = Convert.ToByte(Math.Ceiling((Double)(_projectsService.NumberOfProjects() / PageSize)));
            
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(String projectID)
        {
            await _projectsService.DeleteProject(projectID);
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostSaveNewProjectAsync(String projectName)
        {
            Project newProject = new Project
            {
                ProjectName = projectName
            };

            await _projectsService.SaveNewProject(newProject);

            return RedirectToPage("Index");
        }
    }
}
