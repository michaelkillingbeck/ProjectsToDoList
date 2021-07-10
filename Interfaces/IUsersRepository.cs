namespace ProjectsToDoList.Interfaces
{
    using ProjectsToDoList.Models;
    using System;
    using System.Threading.Tasks;

    public interface IUsersRepository
    {
        Task<Boolean> CreateAsync(User user);
        Task<User> FindByNameAsync(String userName);
    }
}