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

    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CreateModel> _logger;
        private readonly Int32 _pageSize = 17;
        private readonly IProjectsService _projectsService;

        [BindProperty]
        public NewProjectWithTasks NewProject { get; set; }
        public Int32 PageSize => _pageSize;

        public CreateModel(IConfiguration configuration,
                            ILogger<CreateModel> logger,
                            IProjectsService projectsService)
        {            
            _configuration = configuration;
            _logger = logger;
            _projectsService = projectsService;
        }

        public void OnGet(NewProjectWithTasks newProject = null)
        {
            NewProject = newProject;
            if(NewProject.ProjectTasks == null)
            {
                _logger.LogInformation($"{NewProject.ProjectName} has no tasks");
                NewProject.ProjectTasks = new List<String>();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(ModelState.IsValid == false)
            {
                _logger.LogWarning($"{ModelState.ErrorCount} errors trying to save {NewProject.ProjectName}");
                return RedirectToPage("Create", NewProject);
            }

            await _projectsService.SaveNewProjectWithTasks(NewProject);
            _logger.LogInformation($"Saved {NewProject.ProjectName} successfully");
            return RedirectToPage("Index");
        }
    }
}