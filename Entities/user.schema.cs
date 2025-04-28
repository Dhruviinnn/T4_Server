using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TimeFourthe.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Role { get; set; }
        public string? OrgId { get; set; } = null;
        public string? Class { get; set; } = null;
        public List<int>? OrgType { get; set; } = null;
        public List<Schedule>? Schedule { get; set; }
    }
    public class Schedule
    {
        public int StartTime { get; set; }
        public int Day { get; set; }
        public string ClassName { get; set; }
        public string Subject { get; set; }
        public bool IsLab { get; set; }
        public int Duration { get; set; }
        public string? TeacherId { get; set; }
    }
}