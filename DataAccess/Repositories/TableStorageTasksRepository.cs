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
    using System.Threading.Tasks;

    public class TableStorageTasksRepository : ITasksRepository
    {
        private readonly ICloudTableHelper _cloudTableHelper;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public TableStorageTasksRepository(ICloudStorageAccountHelper storageHelper,
                                    IConfiguration configuration,
                                    ILogger<TableStorageTasksRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;

            String connectionString = configuration["ConnectionString"];
            CloudStorageAccount storageAccount = storageHelper.CreateFromConnectionString(connectionString);
            _cloudTableHelper = new CloudTableHelper(storageAccount);
        }

        public async Task SaveAll(IEnumerable<ProjectTask> tasks)
        {
            CloudTable table = await _cloudTableHelper.GetCloudTableByName(_configuration["TasksTableName"]);
            TableBatchOperation batchOperation = new TableBatchOperation();

            foreach(ProjectTask task in tasks)
            {
                batchOperation.Insert(task);
            }

            await table.ExecuteBatchAsync(batchOperation);
        }
    }
}