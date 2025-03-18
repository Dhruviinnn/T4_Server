using MongoDB.Driver;
using TimeFourthe.Entities;
using TimeFourthe.Configurations;
using Microsoft.Extensions.Options;
using IdGenerator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using AuthString;
using Microsoft.AspNetCore.Http;

namespace TimeFourthe.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _usersCollection = database.GetCollection<User>(mongoDbSettings.Value.CollectionName[0]);
        }

        public async Task<User?> GetUserAsync(string email) =>
            await _usersCollection.Find(user => user.Email == email).FirstOrDefaultAsync();

        public async Task CreateUserAsync(User user)
        {
            user.UserId = new IdGeneratorClass().IdGenerator(user.Role);
            try
            {
                await _usersCollection.InsertOneAsync(user);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public async Task<List<User>> GetTechersByOrgIdAsync(string orgId) =>
            await _usersCollection.Find(user => user.OrgId == orgId && user.Role == "teacher").ToListAsync();
        public async Task<List<User>> GetStudentsByOrgIdAsync(string orgId) =>
            await _usersCollection.Find(user => user.OrgId == orgId && user.Role == "student").ToListAsync();
        public async Task<User> GetOrganizationByOrgId(string orgId) =>
            await _usersCollection.Find(user => user.OrgId == orgId).FirstOrDefaultAsync();
    }
}