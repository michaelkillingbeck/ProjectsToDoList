namespace ProjectsToDoList.DataAccess.Repositories
{
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using ProjectsToDoList.DataAccess.TableStorage;
    using ProjectsToDoList.Interfaces;
    using ProjectsToDoList.Models;
    using System;
    using System.Threading.Tasks;

    public class TableStorageUsersRepository : IUsersRepository
    {
        private readonly ICloudTableHelper _cloudTableHelper;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly String _partitionKey = "User";

        public TableStorageUsersRepository(ICloudStorageAccountHelper storageHelper,
                                    IConfiguration configuration,
                                    ILogger<TableStorageUsersRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;

            String connectionString = configuration["ConnectionString"];
            CloudStorageAccount storageAccount = storageHelper.CreateFromConnectionString(connectionString);
            _cloudTableHelper = new CloudTableHelper(storageAccount);
        }

        public async Task<Boolean> CreateAsync(User user)
        {
            TableStorageUser userToStore = new TableStorageUser
            {
                HashedPassword = user.PasswordHash,
                PartitionKey = _partitionKey,
                RowKey = user.NormalizedUserName.ToUpper(),
                Timestamp = DateTime.Now,
                UserName = user.UserName,
            };

            CloudTable table = await _cloudTableHelper.GetCloudTableByName(_configuration["UsersTable"]);
            TableStorageUser savedUser = await _cloudTableHelper.InsertEntityAsync(table, userToStore);

            return savedUser != null;
        }

        public async Task<User> FindByNameAsync(String userName)
        {
            CloudTable table = await _cloudTableHelper.GetCloudTableByName(_configuration["UsersTable"]);
            TableStorageUser foundUser = 
                await _cloudTableHelper.GetEntity<TableStorageUser>(table, _partitionKey, userName);
            
            if(foundUser == null)
            {
                return default;
            }

            User newUser = new User
            {
                PasswordHash = foundUser.HashedPassword,
                UserName = foundUser.UserName
            };

            return newUser;
        }
    }
}