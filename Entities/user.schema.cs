using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TimeFourthe.Entities {
    public class User {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Role { get; set; }
        public string? OrgId { get; set; }=null;
    }
}