namespace ProjectsToDoList.Models
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Cosmos.Table;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProjectWithTasks : TableEntity
    {
        public ProjectWithTasks()
        {
        }

        [BindProperty]
        [Required(ErrorMessage="Project Name is required")]
        public String ProjectName { get; set; }

        [BindProperty]
        public IEnumerable<String> ProjectTasks { get; set; }
    }
}