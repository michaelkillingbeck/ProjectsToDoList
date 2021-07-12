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
    using System.Net;
    using System.Threading.Tasks;

    public class TableStorageProjectsRepository : IProjectsRepository
    {
        private readonly ICloudTableHelper _cloudTableHelper;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly String _partitionKey = "Project";

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

        public async Task<Boolean> DeleteAsync(String projectID)
        {
            CloudTable table = await _cloudTableHelper.GetCloudTableByName(_configuration["TableName"]);
            Project project = new Project
            {
                ETag = "*",
                PartitionKey = _partitionKey,
                RowKey = projectID
            };

            TableResult result = await _cloudTableHelper.DeleteAsync<Project>(table, project);

            return result.HttpStatusCode >= (Int32)HttpStatusCode.OK && result.HttpStatusCode <= (Int32)HttpStatusCode.IMUsed;
        }

        public IEnumerable<Project> GetAll()
        {
            CloudTable table = _cloudTableHelper.GetCloudTableByName(_configuration["TableName"]).Result;
            IEnumerable<Project> allProjects = _cloudTableHelper.GetAllEntities<Project>(table);

            return allProjects;
        }

        public async Task<IEnumerable<Project>> GetPage(Int32 pageNumber, Int32 pageSize)
        {
            List<Project> projects = GetAll().Skip(pageNumber * pageSize).Take(pageSize).ToList();

            foreach(Project project in projects)
            {
                project.ID = project.RowKey;
            }

            return projects;
        }

        public async Task<ExistingProjectWithTasks> GetProjectByName(String name)
        {
            CloudTable table = await _cloudTableHelper.GetCloudTableByName(_configuration["TableName"]);
            
            return await _cloudTableHelper.GetEntity<ExistingProjectWithTasks>(table, _partitionKey, name);
        }

        public async Task Save(Project project)
        {
            project.PartitionKey = _partitionKey;
            project.Timestamp = DateTime.Now;
            project.RowKey = project.ProjectName;
            CloudTable table = await _cloudTableHelper.GetCloudTableByName(_configuration["TableName"]);
            Project savedProject = await _cloudTableHelper.InsertEntityAsync(table, project);
        }

        public async Task Update(Project project)
        {
            project.PartitionKey = _partitionKey;

            CloudTable table = await _cloudTableHelper.GetCloudTableByName(_configuration["TableName"]);
            Project updatedProject = await _cloudTableHelper.UpdateEntityAsync(table, project);
        }
    }
}