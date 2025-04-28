using MongoDB.Driver;
using TimeFourthe.Entities;
using TimeFourthe.Configurations;
using Microsoft.Extensions.Options;
using IdGenerator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using AuthString;
using Microsoft.AspNetCore.Http;
using TimeFourthe.Controllers;

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
        public async Task<User> GetClassesByOrgId(string orgId) =>
            await _usersCollection.Find(user => user.UserId == orgId).FirstOrDefaultAsync();
        public async Task<User> GetOrgNameByOrgId(string orgId) =>
            await _usersCollection.Find(user => user.UserId == orgId).FirstOrDefaultAsync();
        public async Task<List<User>> GetStudentsByOrgIdAndClassAsync(AbsentDataRequest absentData) =>
            await _usersCollection.Find(user => user.OrgId == absentData.OrgId && user.Class == absentData.Class && user.Role == "student").ToListAsync();
        public async Task<User> GetOrganizationByOrgId(string orgId) =>
            await _usersCollection.Find(user => user.UserId == orgId).FirstOrDefaultAsync();
        public async Task<User> GetTeacherScheduleListAsync(string teacherId) =>
            await _usersCollection.Find(user => user.UserId == teacherId).FirstOrDefaultAsync();
        public async Task<bool> UpdateUserAsync(string email, string newPassword)
        {
            var filter = Builders<User>.Filter.Eq(user => user.Email, email);
            var update = Builders<User>.Update.Set(s => s.Password, newPassword);
            var x = await _usersCollection.UpdateOneAsync(filter, update);
            return x.ModifiedCount > 0;
        }

        public async Task<bool> AddScheduleToTeacher(string teacherId, Schedule schedule)
        {
            var filter = Builders<User>.Filter.Eq(user => user.UserId, teacherId);
            var update = Builders<User>.Update.Push(u => u.Schedule, schedule);
            var x = await _usersCollection.UpdateOneAsync(filter, update);
            return x.ModifiedCount > 0;
        }
    }
}