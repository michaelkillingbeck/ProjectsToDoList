namespace ProjectsToDoList.DataAccess.TableStorage
{
    using Microsoft.Azure.Cosmos.Table;
    using ProjectsToDoList.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CloudTableHelper : ICloudTableHelper
    {
        private readonly CloudStorageAccount _storageAccount;

        public CloudTableHelper(CloudStorageAccount storageAccount)
        {
            _storageAccount = storageAccount;                
        }

        public IEnumerable<T> GetAllEntities<T>(CloudTable table) where T : TableEntity, new()
        {
            try
            {
                TableQuery<T> query = new TableQuery<T>();
                var results = table.ExecuteQuery(query);

                return results;
            }
            catch(Exception)
            {
                Console.WriteLine("Failure performing Retrieve operation.");
                throw;
            }
        }

        public async Task<CloudTable> GetCloudTableByName(String tableName)
        {
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference(tableName);

            if(await table.ExistsAsync() == false)
            {
                Console.WriteLine($"Could not find Table named {tableName}.");
                throw new ApplicationException($"Could not find Table named {tableName}.");
            }

            return table;
        }

        public async Task<T> GetEntity<T>(CloudTable table, String partitionKey, String rowKey) where T : TableEntity
        {
            try
            {
                TableOperation operation = TableOperation.Retrieve<T>(partitionKey, rowKey);
                TableResult result = await table.ExecuteAsync(operation);
                T resultEntity = result.Result as T;

                return resultEntity;
            }
            catch(Exception)
            {
                Console.WriteLine("Failure performing Retrieve operation.");
                throw;
            }
        }

        public async Task<T> InsertEntityAsync<T>(CloudTable table, T entity) where T : TableEntity
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                TableOperation operation = TableOperation.Insert(entity);
                TableResult result = await table.ExecuteAsync(operation);

                T returnedEntity = result.Result as T;

                return returnedEntity;
            }
            catch(Exception)
            {
                Console.WriteLine("Failure performing Insert/Merge operation.");
                throw;
            }
        }
    }
}