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
        private readonly ILogger<EditModel> _logger;
        private Boolean _nextPageAvailable;
        private readonly IProjectsService _projectsService;

        [BindProperty]
        public Int32 CurrentPage { get; set; }
        [BindProperty]
        public ExistingProjectWithTasks CurrentProject { get; set; }
        public Boolean NextPageAvailable => _nextPageAvailable;
        public Int32 PageSize => 17;

        public EditModel(IConfiguration configuration,
                            ILogger<EditModel> logger,
                            IProjectsService projectsService)
        {            
            _configuration = configuration;
            CurrentPage = 0;
            _logger = logger;
            _projectsService = projectsService;
        }

        public async Task<IActionResult> OnPostDelete(String taskID, String projectName)
        {
            return RedirectToAction("Get", new { projectName = projectName });
        }

        public async Task<IActionResult> OnGetAsync(String projectName, Int32 pageNumber = 0)
        {
            CurrentPage = pageNumber;
            CurrentProject = await _projectsService.GetProjectByName(projectName.ToLower(), pageNumber, PageSize);
            _nextPageAvailable = CurrentProject.NumberOfTasks - (pageNumber * PageSize) > PageSize;

            return Page();
        }

        public async Task<IActionResult> OnPostNextPageAsync()
        {
            await _projectsService.UpdateCurrentProject(CurrentProject);

            return RedirectToAction("Get", new 
            { 
                projectName = CurrentProject.ProjectName, 
                pageNumber = CurrentPage + 1 
            });
        }

        public async Task<IActionResult> OnPostSaveNewTaskAsync(String projectName, String taskName)
        {
            await _projectsService.SaveNewTask(projectName.ToLower(), taskName);

            return RedirectToPage("Edit", new { projectName });
        }

        public async Task<IActionResult> OnPostPreviousPageAsync()
        {
            await _projectsService.UpdateCurrentProject(CurrentProject);

            return RedirectToAction("Get", new 
            { 
                projectName = CurrentProject.ProjectName, 
                pageNumber = CurrentPage - 1 
            });
        }
    }
}