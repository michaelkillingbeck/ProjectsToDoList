namespace ProjectsToDoList.Interfaces
{
    using ProjectsToDoList.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProjectsService
    {
        Task DeleteProject(String projectID);
        Task DeleteTask(String taskID, String projectName);
        IEnumerable<Project> GetAll();
        IEnumerable<Project> GetPage(Int32 pageNumber, Int32 pageSize);
        Task<ExistingProjectWithTasks> GetProjectByName(String name, Int32 pageNumber, Int32 pageSize);
        Int32 NumberOfProjects();
        Task SaveNewProject(Project newProject);
        Task SaveNewProjectWithTasks(NewProjectWithTasks newProject);
        Task SaveNewTask(String projectName, String taskName);
        Task UpdateCurrentProject(ExistingProjectWithTasks project);
    }
}