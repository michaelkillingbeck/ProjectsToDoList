namespace ProjectsToDoList.Pages
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using ProjectsToDoList.Interfaces;
    using ProjectsToDoList.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private Int32 _currentPage;
        private readonly ILogger<EditModel> _logger;
        private Boolean _nextPageAvailable;
        private readonly IProjectsService _projectsService;


        public Int32 CurrentPage => _currentPage;
        public ExistingProjectWithTasks CurrentProject { get; set; }
        public Boolean NextPageAvailable => _nextPageAvailable;
        public Int32 PageSize => 17;

        public EditModel(IConfiguration configuration,
                            ILogger<EditModel> logger,
                            IProjectsService projectsService)
        {            
            _configuration = configuration;
            _currentPage = 0;
            _logger = logger;
            _projectsService = projectsService;
        }

        public async Task<IActionResult> OnGetAsync(String projectName, Int32 pageNumber = 0)
        {
            _currentPage = pageNumber;
            CurrentProject = await _projectsService.GetProjectByName(projectName.ToLower());
            _nextPageAvailable = CurrentProject.ProjectTasks.Count() - (pageNumber * PageSize) > PageSize;
            CurrentProject.ProjectTasks = CurrentProject.ProjectTasks.Skip(pageNumber * PageSize).Take(PageSize);

            return Page();
        }

        public async Task<IActionResult> OnPostSaveNewTaskAsync(String projectName, String taskName)
        {
            await _projectsService.SaveNewTask(projectName.ToLower(), taskName);

            return RedirectToPage("Edit", new { projectName });
        }
    }
}