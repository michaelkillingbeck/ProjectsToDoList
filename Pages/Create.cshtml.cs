namespace ProjectsToDoList.Pages
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using ProjectsToDoList.Models;
    using System;
    using System.Collections.Generic;

    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CreateModel> _logger;
        private readonly Int32 _pageSize = 17;

        [BindProperty]
        public ProjectWithTasks NewProject { get; set; }
        public Int32 PageSize => _pageSize;

        public CreateModel(IConfiguration configuration,
                            ILogger<CreateModel> logger)
        {            
            _configuration = configuration;
            _logger = logger;

            NewProject = new ProjectWithTasks();
            NewProject.ProjectTasks = new List<String>();
        }

        public void OnGet(ProjectWithTasks newProject = null)
        {
            NewProject = newProject;
            if(NewProject.ProjectTasks == null)
            {
                NewProject.ProjectTasks = new List<String>();
            }
        }

        public IActionResult OnPost()
        {
            if(ModelState.IsValid == false)
            {
                return RedirectToPage("Create", NewProject);
            }

            return RedirectToPage("Index");
        }
    }
}