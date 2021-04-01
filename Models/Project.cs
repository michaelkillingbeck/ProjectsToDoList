namespace ProjectsToDoList.Models
{
    using Microsoft.Azure.Cosmos.Table;
    using System;

    public class Project : TableEntity
    {
        public Project()
        {
        }

        public String ProjectName { get; set; }
    }
}