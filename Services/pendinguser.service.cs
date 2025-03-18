using MongoDB.Driver;
using TimeFourthe.Entities;
using TimeFourthe.Configurations;
using Microsoft.Extensions.Options;
using IdGenerator;

namespace TimeFourthe.Services
{
    public class PendingUserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public PendingUserService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _usersCollection = database.GetCollection<User>(mongoDbSettings.Value.CollectionName[1]);
        }

        public async Task<List<string>> CreatePendingUserAsync(User user)
        {
            user.UserId = new IdGeneratorClass().IdGenerator("organization", 15);
            await _usersCollection.InsertOneAsync(user);
            return [user.UserId, user.Name];
        }
        public async Task<User> GetPendingUserAsync(string email) =>
                    await _usersCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
        public async Task<User> DeletePendingUserAsync(string orgId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserId, orgId);
            return await _usersCollection.FindOneAndDeleteAsync(filter);
        }
    }
}