namespace ProjectsToDoList.Models
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Cosmos.Table;
    using System;

    public class ProjectTaskEntity : TableEntity
    {
        [BindProperty]
        public Boolean Completed { get; set; }
        
        [BindProperty]
        public String TaskName { get; set; }

        public ProjectTaskEntity()
        {            
        }
    }
}