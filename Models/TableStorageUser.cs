namespace ProjectsToDoList.Models
{
    using Microsoft.Azure.Cosmos.Table;
    using System;

    public class TableStorageUser : TableEntity
    {
        public String HashedPassword { get; set; }
        public String UserName { get; set; }

        public TableStorageUser()
        {
        }
    }
}