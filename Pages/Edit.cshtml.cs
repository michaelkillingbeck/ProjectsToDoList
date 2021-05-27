namespace ProjectsToDoList.Pages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using ProjectsToDoList.Interfaces;
    using ProjectsToDoList.Models;
    using System.Threading.Tasks;
    using System;

    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EditModel> _logger;
        private readonly IProjectsService _projectsService;

        public ExistingProjectWithTasks CurrentProject { get; set; }
        public Int32 PageSize => 19;

        public EditModel(IConfiguration configuration,
                            ILogger<EditModel> logger,
                            IProjectsService projectsService)
        {            
            _configuration = configuration;
            _logger = logger;
            _projectsService = projectsService;
        }

        public async Task OnGetAsync(String projectName)
        {
            CurrentProject = await _projectsService.GetProjectByName(projectName);
        }
    }
}