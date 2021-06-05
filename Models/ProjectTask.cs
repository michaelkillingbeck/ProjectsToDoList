namespace ProjectsToDoList.Models
{
    using Microsoft.AspNetCore.Mvc;

    public class ProjectTask : ProjectTaskEntity
    {
        [BindProperty]
        public string ID { get; set; }

        public static ProjectTask CreateFromEntity(ProjectTaskEntity original)
        {
            ProjectTask newTask = new ProjectTask();

            newTask.ID = original.RowKey;
            newTask.PartitionKey = original.PartitionKey;
            newTask.RowKey = original.RowKey;
            newTask.TaskName = original.TaskName;
            newTask.Timestamp = original.Timestamp;

            return newTask;
        }
    }
}