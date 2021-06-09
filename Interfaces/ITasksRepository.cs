namespace ProjectsToDoList.Interfaces
{
    using ProjectsToDoList.Models;
    using System.Collections.Generic;
    using System;
    using System.Threading.Tasks;

    public interface ITasksRepository
    {
        Task Delete(String taskID, String projectName);
        Task DeleteAllTasksForProjectAsync(String projectID);
        Task<IEnumerable<ProjectTaskEntity>> GetTasksForProject(String name);
        Task SaveAll(IEnumerable<ProjectTaskEntity> tasks);
        Task SaveNewTask(ProjectTaskEntity newTask);
        Task UpdateAll(IEnumerable<ProjectTaskEntity> tasks);
    }
}