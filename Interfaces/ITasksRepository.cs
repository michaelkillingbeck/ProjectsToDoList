namespace ProjectsToDoList.Interfaces
{
    using ProjectsToDoList.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITasksRepository
    {
        Task SaveAll(IEnumerable<ProjectTask> tasks);
    }
}