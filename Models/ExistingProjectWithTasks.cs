namespace ProjectsToDoList.Models
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;

    public class ExistingProjectWithTasks : Project
    {
        public Int32 NumberOfTasks { get; set; }

        [BindProperty]
        public List<ProjectTask> ProjectTasks { get; set; }
        
        public ExistingProjectWithTasks()
        {
        }
    }
}