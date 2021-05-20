namespace ProjectsToDoList.Models
{
    using Microsoft.Azure.Cosmos.Table;
    using System;
    using System.Collections.Generic;

    public class ProjectWithTasks : TableEntity
    {
        public ProjectWithTasks()
        {
        }

        public String ProjectName { get; set; }
        public IEnumerable<String> ProjectTasks { get; set; }
    }
}