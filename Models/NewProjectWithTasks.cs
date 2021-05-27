namespace ProjectsToDoList.Models
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;

    public class NewProjectWithTasks : Project
    {
        public NewProjectWithTasks()
        {
        }

        [BindProperty]
        public IEnumerable<String> ProjectTasks { get; set; }
    }
}