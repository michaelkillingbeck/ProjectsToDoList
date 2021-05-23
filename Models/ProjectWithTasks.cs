namespace ProjectsToDoList.Models
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Cosmos.Table;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProjectWithTasks : Project
    {
        public ProjectWithTasks()
        {
        }

        [BindProperty]
        public IEnumerable<String> ProjectTasks { get; set; }
    }
}