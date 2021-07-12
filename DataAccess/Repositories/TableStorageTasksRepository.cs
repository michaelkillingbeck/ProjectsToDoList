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

        public async Task<Boolean> AllTasksAreCompleted(String projectName)
        {
            IEnumerable<ProjectTaskEntity> tasks = await GetTasksForProject(projectName);

            return tasks.All(task => task.Completed);
        }

        public async Task Delete(String taskID, String projectName)
        {
            CloudTable table = await _cloudTableHelper.GetCloudTableByName(_configuration["TasksTableName"]);
            ProjectTask task = new ProjectTask
            {
                ETag = "*",
                PartitionKey = projectName,
                RowKey = taskID
            };

            await _cloudTableHelper.DeleteAsync<ProjectTask>(table, task);
        }

        public async Task DeleteAllTasksForProjectAsync(String projectID)
        {
            IEnumerable<ProjectTaskEntity> tasksToDelete = await GetTasksForProject(projectID);

            foreach(ProjectTaskEntity taskToDelete in tasksToDelete)
            {
                await Delete(taskToDelete.RowKey, projectID);
            }
        }

        public async Task<IEnumerable<ProjectTaskEntity>> GetTasksForProject(String projectName)
        {
            CloudTable table = await _cloudTableHelper.GetCloudTableByName(_configuration["TasksTableName"]);
            return _cloudTableHelper.GetAllEntitiesByPartitionKey<ProjectTaskEntity>(table, projectName);
        }

        public async Task SaveAll(IEnumerable<ProjectTaskEntity> tasks)
        {
            CloudTable table = await _cloudTableHelper.GetCloudTableByName(_configuration["TasksTableName"]);
            TableBatchOperation batchOperation = new TableBatchOperation();

            foreach(ProjectTaskEntity task in tasks)
            {
                batchOperation.Insert(task);
            }

            await table.ExecuteBatchAsync(batchOperation);
        }

        public async Task SaveNewTask(ProjectTaskEntity newTask)
        {
            CloudTable table = await _cloudTableHelper.GetCloudTableByName(_configuration["TasksTableName"]);
            await _cloudTableHelper.InsertEntityAsync<ProjectTaskEntity>(table, newTask);
        }

        public async Task UpdateAll(IEnumerable<ProjectTaskEntity> tasks)
        {
            CloudTable table = await _cloudTableHelper.GetCloudTableByName(_configuration["TasksTableName"]);
            TableBatchOperation batchOperation = new TableBatchOperation();

            foreach(ProjectTaskEntity task in tasks)
            {
                batchOperation.InsertOrMerge(task);
            }

            await table.ExecuteBatchAsync(batchOperation);
        }
    }
}