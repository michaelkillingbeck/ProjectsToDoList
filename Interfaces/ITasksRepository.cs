namespace ProjectsToDoList.Interfaces
{
    using ProjectsToDoList.Models;
    using System.Collections.Generic;
    using System;
    using System.Threading.Tasks;

    public interface ITasksRepository
    {
        Task<IEnumerable<ProjectTask>> GetTasksForProject(String name);
        Task SaveAll(IEnumerable<ProjectTask> tasks);
        Task SaveNewTask(ProjectTask newTask);
    }
}