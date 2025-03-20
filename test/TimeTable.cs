using System;
using System.Collections.Generic;
using System.Reflection;

public class TeacherStartTime
{
    public int? StartTime { get; set; }
    public string? TeacherId { get; set; }
}
public class Teacher
{
    public string? Name { get; set; }
    public string? TeacherId { get; set; }
}

public class Subject
{
    public string? Name { get; set; }
    public Teacher? Teacher { get; set; }
}

public class Period
{
    public int StartTime { get; set; }
    public Subject? Subject { get; set; }
    public bool IsLab { get; set; } = false;
}

public class TimetableSchema
{
    public required string OrgId { get; set; }
    public required string Class { get; set; }
    public required string Division { get; set; }
    public required int Year { get; set; }
    public int? StartTime { get; set; }
    public int HoursPerDay { get; set; }
    public int PeriodDuration { get; set; }
    public int BreakDuration { get; set; }
    public int LabDuration { get; set; }
    public List<List<Period>>? Timetable { get; set; }
    public List<Subject>? Subjects { get; set; }
}

public class Schedule
{
    public int StartTime { get; set; }
    public int Day { get; set; }
}
// -----------------------------------------------------------------------------------------------




public class Program
{
    static List<Subject> subjects = new List<Subject> // Arrive from frontend
        {
            new Subject { Name = "Math", Teacher = new Teacher { Name = "John Doe", TeacherId = "TCH1001" } },
            new Subject { Name = "Physics", Teacher = new Teacher { Name = "Jane Smith", TeacherId = "TCH1002" } },
            new Subject { Name = "Chemistry", Teacher = new Teacher { Name = "Mike Johnson", TeacherId = "TCH1003" } },
            new Subject { Name = "Biology", Teacher = new Teacher { Name = "Sarah Lee", TeacherId = "TCH1044" } },
            new Subject { Name = "History", Teacher = new Teacher { Name = "Emily Davis", TeacherId = "TCH1005" } },
            new Subject { Name = "Science", Teacher = new Teacher { Name = "Mickey Sen", TeacherId = "TCH1001" } }
        };

    static List<Schedule> scheduleListForTeacher = new List<Schedule>{ // Fetching from DB using teacher ID, this type of list for each teacher
        // TCH206533240295 - Frank_Physics
        new Schedule { StartTime = 600, Day = 0},
        new Schedule { StartTime = 660, Day = 1 },
        new Schedule { StartTime = 600, Day = 2},
        new Schedule { StartTime = 780, Day = 3 },
        new Schedule { StartTime = 600, Day = 4},
        new Schedule { StartTime = 780, Day = 5 }
    };

    static TimetableSchema x = new TimetableSchema // Arrive from frontend
    {
        OrgId = "ORG983156005450",
        Class = "Class X",
        Division = "A",
        Year = 2024,
        StartTime = 540,
        HoursPerDay = 6,
        PeriodDuration = 45,
        BreakDuration = 10,
        LabDuration = 90,
        Timetable = null
    };

    static bool isTeacherAvailable(int day, int time, string teacherId)
    {
        var tmp = scheduleListForTeacher.Find(item => item.Day == day && time >= item.StartTime && time <= item.StartTime + x.PeriodDuration);
        return tmp == null;
    }

    static List<List<Period>> GenerateTimetableSchema(List<Subject> subjects)
    {
        string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        List<List<Period>> tt = new List<List<Period>>();

        int[] periodsPerDay = { 4, 5, 5, 5, 5, 5 };
        int breakDurationInMinutes = 60;
        int hoursPerDay = 5;
        int hoursPerDayInMinutes = (hoursPerDay * 60) - breakDurationInMinutes;
        int startTime = 540; // 9:00 AM (in minutes)
        int periodDuration = 60; // Each class is 60 minutes

        Random rand = new Random();
        Dictionary<string, HashSet<string>> teacherSchedule = new Dictionary<string, HashSet<string>>();

        // Generate TimetableSchema
        for (int i = 0; i < days.Length; i++)
        {
            string day = days[i];
            int periods = periodsPerDay[i];
            int currentStartTime = startTime;

            if (!(hoursPerDayInMinutes <= periods * periodDuration))
            {
                return [];
            }
            else
            {
                for (int j = 0; (currentStartTime - startTime) <= hoursPerDayInMinutes; j++)
                {
                    Subject subject;
                    Teacher teacher;

                    do
                    {
                        subject = subjects[rand.Next(subjects.Count)];
                        teacher = subject.Teacher;
                    } while (teacherSchedule.ContainsKey(day) && teacherSchedule[day].Contains(teacher.TeacherId));

                    // Assign the teacher to this time slot
                    if (isTeacherAvailable(i, currentStartTime, teacher.TeacherId))
                    {
                        if (!teacherSchedule.ContainsKey(day))
                        {
                            teacherSchedule[day] = new HashSet<string>();
                        }
                        teacherSchedule[day].Add(teacher.TeacherId);
                    }
                    // Add period to TimetableSchema
                    try
                    {
                        tt[i].Add(new Period { StartTime = currentStartTime, Subject = subject });
                    }
                    catch (System.Exception)
                    {
                        tt.Add(new List<Period>());
                        // throw;
                    }
                    currentStartTime += periodDuration;
                }
            }
        }
        return tt;
    }

    static string justForPrint(int index)
    {
        switch (index)
        {
            case 0: return "Monday";
            case 1: return "Tuesday";
            case 2: return "Wednesday";
            case 3: return "Thursday";
            case 4: return "Friday";
            case 5: return "Saturday";
        }
        return "";
    }
    static void Main()
    {
        // Console.WriteLine(isTeacherAvailable(0, 646, "TCH206533240295"));
        List<List<Period>> x = GenerateTimetableSchema(subjects);
        if (x.Count > 0)
        {
            foreach (var day in x)
            {
                Console.WriteLine($"{justForPrint(x.IndexOf(day))} : ");
                foreach (var period in day)
                {
                    Console.WriteLine($"\t\t\tStartTime : {period.StartTime} , Name : {period.Subject.Name} , Subject : {period.Subject.Teacher.Name}");
                }
            }
        }
        else
        {
            Console.WriteLine("TimeTable can't be build");
            Console.WriteLine("Decrease hoursPerDay or Increase periodDuraions");
        }
    }
}
