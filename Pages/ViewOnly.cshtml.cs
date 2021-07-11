namespace ProjectsToDoList.Pages
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using ProjectsToDoList.Interfaces;
    using ProjectsToDoList.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [AllowAnonymous]
    public class ViewOnlyModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IndexModel> _logger;
        private readonly IProjectsService _projectsService;
        private readonly Int32 _pageSize = 18;
        
        public Int32 PageSize => _pageSize;
        public IEnumerable<Project> Projects { get; set; }

        public ViewOnlyModel(IConfiguration configuration, 
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
            _logger.LogDebug($"Getting View Only projects index");

            Projects = _projectsService.GetPage(0, _pageSize, true);

            _logger.LogInformation($"Found {Projects.ToList().Count} Projects");

            return Page();
        }
    }
}
