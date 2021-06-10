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

        public async Task<IActionResult> OnGetAsync(String projectName, Int32 pageNumber = 0)
        {
            CurrentPage = pageNumber;
            CurrentProject = await _projectsService.GetProjectByName(projectName.ToLower(), pageNumber, PageSize);
            _nextPageAvailable = CurrentProject.NumberOfTasks - (pageNumber * PageSize) > PageSize;

            return Page();
        }
        
        public async Task<IActionResult> OnPostDeleteAsync(String taskID, String projectName)
        {
            _logger.LogInformation($"Deleting {taskID} from {projectName}");
            await _projectsService.DeleteTask(taskID, projectName);

            _logger.LogInformation($"Deleted {taskID}");
            return RedirectToAction("Get", new { projectName = projectName });
        }

        public async Task<IActionResult> OnPostNextPageAsync()
        {
            _logger.LogInformation($"Navigating to next page; Current Page is {CurrentPage}");
            _logger.LogInformation($"Updating {CurrentProject.ProjectName}");
            _logger.LogInformation($"{CurrentProject.ProjectName} has {CurrentProject.ProjectTasks.Count} tasks");

            await _projectsService.UpdateCurrentProject(CurrentProject);
            _logger.LogInformation($"Updated {CurrentProject.ProjectName} successfully");

            return RedirectToAction("Get", new 
            { 
                projectName = CurrentProject.ProjectName, 
                pageNumber = CurrentPage + 1 
            });
        }

        public async Task<IActionResult> OnPostSaveNewTaskAsync(String projectName, String taskName)
        {
            _logger.LogInformation($"Saving {taskName} for {projectName}");
            await _projectsService.SaveNewTask(projectName.ToLower(), taskName);

            _logger.LogInformation($"{taskName} saved successfully");
            return RedirectToPage("Edit", new { projectName });
        }

        public async Task<IActionResult> OnPostPreviousPageAsync()
        {
            _logger.LogInformation($"Navigating to previous page; Current Page is {CurrentPage}");
            _logger.LogInformation($"Updating {CurrentProject.ProjectName}");
            _logger.LogInformation($"{CurrentProject.ProjectName} has {CurrentProject.ProjectTasks.Count} tasks");

            await _projectsService.UpdateCurrentProject(CurrentProject);
            _logger.LogInformation($"Updated {CurrentProject.ProjectName} successfully");

            return RedirectToAction("Get", new 
            { 
                projectName = CurrentProject.ProjectName, 
                pageNumber = CurrentPage - 1 
            });
        }
    }
}