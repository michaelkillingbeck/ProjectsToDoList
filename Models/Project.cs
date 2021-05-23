namespace ProjectsToDoList.Models
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Cosmos.Table;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Project : TableEntity
    {
        public Project()
        {
        }

        [BindProperty]
        [Required(ErrorMessage="Project Name is required")]
        public String ProjectName { get; set; }
    }
}