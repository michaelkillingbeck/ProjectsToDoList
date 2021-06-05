namespace ProjectsToDoList.Interfaces
{
    using ProjectsToDoList.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProjectsRepository
    {
        IEnumerable<Project> GetAll();
        IEnumerable<Project> GetPage(Int32 pageNumber, Int32 pageSize);
        Task<ExistingProjectWithTasks> GetProjectByName(String name);
        Task Save(Project project);
        Task Update(Project project);
    }
}