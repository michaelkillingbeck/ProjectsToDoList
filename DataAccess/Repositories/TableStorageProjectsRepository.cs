namespace ProjectsToDoList.DataAccess.Repositories
{
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using ProjectsToDoList.DataAccess.TableStorage;
    using ProjectsToDoList.Interfaces;
    using ProjectsToDoList.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TableStorageProjectsRepository : IProjectsRepository
    {
        private readonly ICloudTableHelper _cloudTableHelper;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public TableStorageProjectsRepository(ICloudStorageAccountHelper storageHelper,
                                    IConfiguration configuration,
                                    ILogger<TableStorageProjectsRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;

            String connectionString = configuration["ConnectionString"];
            CloudStorageAccount storageAccount = storageHelper.CreateFromConnectionString(connectionString);
            _cloudTableHelper = new CloudTableHelper(storageAccount);
        }

        public IEnumerable<Project> GetAll()
        {
            CloudTable table = _cloudTableHelper.GetCloudTableByName(_configuration["TableName"]).Result;
            return _cloudTableHelper.GetAllEntities<Project>(table);
        }

        public IEnumerable<Project> GetPage(Int32 pageNumber, Int32 pageSize)
        {
            var projects = GetAll().Skip(pageNumber * pageSize).Take(pageSize).ToList();

            FillPage(projects, pageSize);

            return projects;
        }

        private void FillPage(IList<Project> currentPage, Int32 pageSize)
        {
            if(currentPage.Count() >= pageSize)
            {
                return;
            }

            while(currentPage.Count() < pageSize)
            {
                currentPage.Add(new Project
                {
                    ProjectName = "_"
                });
            }
        }
    }
}