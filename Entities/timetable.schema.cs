using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TimeFourthe.Entities
{
    public class TimetableData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public required string OrgId { get; set; }
        public required string Class { get; set; }
        public required string Division { get; set; }
        public required int Year { get; set; }
        public int? StartTime { get; set; }
        public int HoursPerDay { get; set; }
        public int PeriodDuration { get; set; }
        public int BreakDuration { get; set; }
        public int LabDuration { get; set; }
        public Timetable? Timetable { get; set; }
        public List<Subject>? Subjects { get; set; }
    }

    public class Timetable
    {
        public List<Period>? Monday { get; set; }
        public List<Period>? Tuesday { get; set; }
        public List<Period>? Wednesday { get; set; }
        public List<Period>? Thursday { get; set; }
        public List<Period>? Friday { get; set; }
        public List<Period>? Saturday { get; set; }
    }

    public class Period
    {
        public int StartTime { get; set; }
        public Subject? Subject { get; set; }
        public bool IsLab { get; set; }
    }


    public class Subject
    {
        public string? Name { get; set; }
        public Teacher? Teacher { get; set; }

    }

    public class Teacher
    {
        public string? Name { get; set; }
        public string? TeacherId { get; set; }
    }
    public class Schedule
    {
        public int StartTime { get; set; }
        public int Day { get; set; }
    }
}