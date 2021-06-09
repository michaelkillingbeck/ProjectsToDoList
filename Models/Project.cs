namespace ProjectsToDoList.Models
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Cosmos.Table;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Project : TableEntity
    {
        [BindProperty]
        public String ID { get; set; }

        [BindProperty]
        [Required(ErrorMessage="Project Name is required")]
        public String ProjectName { get; set; }

        public Project()
        {
        }
    }
}