namespace ProjectsToDoList.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using ProjectsToDoList.Interfaces;
    using ProjectsToDoList.Models;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AuthenticationService : 
        IUserStore<User>, 
        IRoleStore<Role>,
        IUserPasswordStore<User>
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUsersRepository _usersRepository;

        public AuthenticationService(IUsersRepository usersRepository, IPasswordHasher<User> passwordHasher, ILogger<AuthenticationService> logger)
        {
            _logger = logger;
            _passwordHasher = passwordHasher;
            _usersRepository = usersRepository;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _logger.LogInformation("Request to create new User");
            _logger.LogInformation(user.ToString());
            Boolean result = await _usersRepository.CreateAsync(user);

            IdentityResult identityResult = new IdentityResult();

            if(!result)
            {
                _logger.LogError("Failed to create new User");
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Failed to create user"
                });
            }

            _logger.LogInformation("New User created successfully.");
            return identityResult;
        }

        public Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
        }

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<User> FindByNameAsync(String normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _logger.LogInformation($"Looking up User, Username is {normalizedUserName}");
            User foundUser = await _usersRepository.FindByNameAsync(normalizedUserName);

            if(foundUser == null)
            {
                _logger.LogInformation("Did not find User");
            }

            return foundUser;
        }

        public Task<String> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<String> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<String> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult<String>(user.PasswordHash);
        }

        public Task<String> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<String> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<String> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _logger.LogInformation($"Requesting User Id for {user}");
            return Task.FromResult<String>(user.Id);
        }

        public Task<String> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _logger.LogInformation($"Requesting User Name for {user}");
            if (user == null) 
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult<String>(user.UserName);
        }

        public Task<Boolean> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _logger.LogInformation($"Setting Normalized Username for: {user.ToString()} to {user.UserName.ToUpper()}");
            user.NormalizedUserName = user.UserName.ToUpper();

            return Task.CompletedTask;;
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        Task<Role> IRoleStore<Role>.FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        Task<Role> IRoleStore<Role>.FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}