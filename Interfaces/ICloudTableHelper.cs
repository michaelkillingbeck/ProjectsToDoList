namespace ProjectsToDoList.Interfaces
{
    using Microsoft.Azure.Cosmos.Table;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICloudTableHelper
    {
        Task<TableResult> DeleteAsync<T>(CloudTable table, T entity) where T : TableEntity, new();
        Task<IEnumerable<T>> GetAllEntities<T>(CloudTable table) where T : TableEntity, new();
        IEnumerable<T> GetAllEntitiesByPartitionKey<T>(CloudTable cloudTable, String partitionKey) where T : TableEntity, new();
        Task<CloudTable> GetCloudTableByName(String tableName);
        Task<T> GetEntity<T>(CloudTable table, String partitionKey, String rowKey) where T : TableEntity;
        Task<T> InsertEntityAsync<T>(CloudTable table, T entity) where T : TableEntity;
        Task<T> UpdateEntityAsync<T>(CloudTable table, T entity) where T : TableEntity;
    }
}