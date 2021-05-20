namespace ProjectsToDoList.Pages
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using ProjectsToDoList.Models;
    using System;
    using System.Threading.Tasks;

    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CreateModel> _logger;
        private readonly Int32 _pageSize = 19;

        [BindProperty]
        public ProjectWithTasks NewProject { get; set; }
        public Int32 PageSize => _pageSize;

        public CreateModel(IConfiguration configuration,
                            ILogger<CreateModel> logger)
        {            
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        }
    }
}