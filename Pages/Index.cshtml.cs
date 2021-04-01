namespace ProjectsToDoList.Pages
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using ProjectsToDoList.DataAccess.Repositories;
    using ProjectsToDoList.Interfaces;
    using ProjectsToDoList.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IndexModel> _logger;
        private readonly IProjectsRepository _projectsRepository;

        private Int32 _currentPage = 0;
        private Int32 _pageSize = 15;
        
        public IEnumerable<Project> Projects { get; set; }

        public IndexModel(IConfiguration configuration, 
                            ILogger<IndexModel> logger,
                            IProjectsRepository projectsRepository)
        {
            Projects = new List<Project>();
            _configuration = configuration;
            _logger = logger;
            _projectsRepository = projectsRepository;
        }

        public void OnGet()
        {
            Projects = _projectsRepository.GetPage(_currentPage, _pageSize);
        }
    }
}
