namespace ProjectsToDoList.Models
{
    using Microsoft.Azure.Cosmos.Table;
    using System;

    public class ProjectTask : TableEntity
    {
        public ProjectTask()
        {            
        }

        public Boolean Completed { get; set; }
        public String ProjectRowKey { get; set; }
        public String TaskName { get; set; }
    }
}