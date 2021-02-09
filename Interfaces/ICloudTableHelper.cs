using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectsToDoList.Interfaces
{
    public interface ICloudTableHelper
    {
        IEnumerable<T> GetAllEntities<T>(CloudTable table) where T : TableEntity, new();
        Task<CloudTable> GetCloudTableByName(String tableName);
        Task<T> GetEntity<T>(CloudTable table, String partitionKey, String rowKey) where T : TableEntity;
        Task<T> InsertOrMergeEntityAsync<T>(CloudTable table, T entity) where T : TableEntity;
    }
}