using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using ProjectsToDoList.DataAccess;
using ProjectsToDoList.Interfaces;
using ProjectsToDoList.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectsToDoList.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICloudTableHelper _cloudTableHelper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IndexModel> _logger;
        
        public IEnumerable<Project> Projects { get; set; }

        public IndexModel(IConfiguration configuration, ICloudStorageAccountHelper storageHelper, 
                            ILogger<IndexModel> logger)
        {
            _logger = logger;
            Projects = new List<Project>();
            _configuration = configuration;
            String connectionString = configuration["ConnectionString"];
            CloudStorageAccount storageAccount = storageHelper.CreateFromConnectionString(connectionString);
            _cloudTableHelper = new CloudTableHelper(storageAccount);
        }

        public void OnGet()
        {
            CloudTable table = _cloudTableHelper.GetCloudTableByName(_configuration["TableName"]).Result;
            Projects = _cloudTableHelper.GetAllEntities<Project>(table);
        }
    }
}
