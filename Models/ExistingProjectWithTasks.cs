namespace ProjectsToDoList.Models
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    public class ExistingProjectWithTasks : Project
    {
        public ExistingProjectWithTasks()
        {
        }

        [BindProperty]
        public IEnumerable<ProjectTask> ProjectTasks { get; set; }
    }
}