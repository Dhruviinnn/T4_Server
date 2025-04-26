using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeFourthe.Entities;
using TimeFourthe.Services;

namespace TimeFourthe.Controllers
{
    [Route("api")]
    [ApiController]
    public class TimetablesController : ControllerBase
    {
        private readonly TimetableService _timetableService;
        private readonly UserService _userService;
        private TimetableData generatedTT;
        private List<ScheduleTeacher> scheduledTeachers;
        string[] days;

        public TimetablesController(TimetableService timetableService, UserService userService)
        {
            _timetableService = timetableService;
            _userService = userService;
            days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
        }



        [HttpGet("getch")]
        public async Task<OkObjectResult> XX()
        {
            bool result = await _userService.AddScheduleToTeacher("TCH1001", new Schedule { StartTime = 1000, Day = 0, ClassName = "CSE BE-III" });
            return Ok(new { result });
        }

        [HttpGet("get/timetable")]
        public async Task<OkObjectResult> GetTimeTables()
        {
            var timetablesWholeData = await _timetableService.GetTimetableDataByOrgIdAsync(Request.Query["OrgId"].ToString());
            var timetables = timetablesWholeData.Select(tts => tts.Timetable);
            return Ok(new { timetables });
        }
        [HttpGet("delete/timetable")]
        public async Task<OkObjectResult> DeleteTimeTables()
        {
            await _timetableService.DeleteTimetableAsync(Request.Query["id"].ToString());
            return Ok(new { error = false, result = "TimeTable Deleted" });
        }

        [HttpPost("generate/timetable")]
        public async Task<OkObjectResult> GetTimeTable([FromBody] TimeTableDetails TimeTable)
        {
            List<Subject> subjects = TimeTable.Subjects;
            List<List<Period>> tt = new List<List<Period>>();

            int? HoursPerDayInMinutes = (TimeTable.HoursPerDay * 60) - TimeTable.BreakDuration;
            Random rand = new Random();
            Dictionary<string, HashSet<string>> teacherSchedule = new Dictionary<string, HashSet<string>>();
            HashSet<string> isAllTeacherScheduled;
            HashSet<string> unavailableTeachers;
            scheduledTeachers = new List<ScheduleTeacher>();

            Dictionary<string, List<Schedule>> scheduleListForTeachers = await GetScheduleListForAllTeachers(TimeTable.Subjects.Select(sub => sub.Teacher).DistinctBy(teacher => teacher.TeacherId).ToList());
            for (int i = 0; i < days.Length; i++)
            {
                string day = days[i];
                int currentStartTime = TimeTable.StartTime;
                unavailableTeachers = new HashSet<string>();
                isAllTeacherScheduled = new HashSet<string>();
                if (!(HoursPerDayInMinutes <= subjects.Count * TimeTable.PeriodDuration))
                {
                    return Ok(new { error = "Decrease hoursPerDay or Increase periodDuraions", GeneratedTimeTable = new List<int>() });
                }
                else
                {
                    while (currentStartTime - TimeTable.StartTime < HoursPerDayInMinutes)
                    {
                        Subject subject;
                        Teacher teacher;

                        do
                        {
                            subject = subjects[rand.Next(subjects.Count)];
                            teacher = subject.Teacher;
                        } while (teacherSchedule.ContainsKey(day) && teacherSchedule[day].Contains(teacher.TeacherId));

                        if (teacherSchedule.ContainsKey(day))
                        {
                            if (subjects.Count - teacherSchedule[day].Count - unavailableTeachers.Count <= 0)
                            {
                                Console.WriteLine("In Break Func");
                                break;
                            }
                        }


                        if (isTeacherAvailable(scheduleListForTeachers[teacher.TeacherId], i, currentStartTime, teacher.TeacherId, TimeTable.PeriodDuration, isAllTeacherScheduled, unavailableTeachers))
                        {
                            if (!teacherSchedule.ContainsKey(day))
                            {
                                teacherSchedule[day] = new HashSet<string>();
                            }
                            try
                            {


                                tt[i].Add(new Period { StartTime = currentStartTime, Subject = subject });
                                unavailableTeachers.Remove(subject.Teacher.TeacherId);
                                currentStartTime += subject.IsLab ? TimeTable.LabDuration : TimeTable.PeriodDuration;
                                teacherSchedule[day].Add(teacher.TeacherId);

                                scheduledTeachers.Add(new ScheduleTeacher { StartTime = currentStartTime, ClassName = TimeTable.Class, Day = i, TeacherId = subject.Teacher.TeacherId });

                            }
                            catch (System.Exception)
                            {
                                tt.Add(new List<Period>());
                            }
                        }
                        if (scheduleListForTeachers.Count == isAllTeacherScheduled.Count)
                        {
                            return Ok(new { message = "All teachers are scheduled" });
                        }
                    }
                }
            }
            generatedTT = new TimetableData
            {
                Timetable = tt,
                OrgId = TimeTable.OrgId,
                Class = TimeTable.Class,
                Division = TimeTable.Division,
                Year = TimeTable.Year
            };

            foreach (var item in scheduledTeachers)
            {
                Console.WriteLine($"{item.ClassName} -- {item.Day} -- {item.StartTime} -- {item.TeacherId}");
            }

            return Ok(new
            {
                generatedTT
            });
        }

        private async Task<Dictionary<string, List<Schedule>>> GetScheduleListForAllTeachers(List<Teacher> teachers)
        {
            Dictionary<string, List<Schedule>> x = new Dictionary<string, List<Schedule>>();
            foreach (var item in teachers)
            {
                x.Add(item.TeacherId, (await _userService.GetTeacherScheduleListAsync(item.TeacherId)).Schedule);
            }
            return x;
        }
        private bool isTeacherAvailable(List<Schedule> scheduleListForTeacher, int day, int time, string teacherId, int PeriodDuration, HashSet<String> isAllTeacherScheduled, HashSet<String> unavailableTeachers)
        {
            var tmp = scheduleListForTeacher.Find(item => item.Day == day && time >= item.StartTime && time < item.StartTime + PeriodDuration);
            if (tmp != null) { isAllTeacherScheduled.Add(teacherId); unavailableTeachers.Add(teacherId); }
            return tmp == null;
        }

        [HttpPost("upload/timetable")]
        public async Task<OkObjectResult> UploadTimeTable()
        {

            await _timetableService.InsertTimetableDataAsync(generatedTT);

            foreach (var item in scheduledTeachers)
            {
                await _userService.AddScheduleToTeacher(item.TeacherId, new Schedule { StartTime = item.StartTime, Day = item.Day, ClassName = item.ClassName });
            }
            return Ok(new {result="TimeTable Created"});
        }



        [HttpPost("show/tt")]
        public async Task<OkObjectResult> DeleteTimeTable()
        {
            var tt = await _timetableService.GetTimetableAsync("67dcf92e2318d3633ffcbb85");
            return Ok(new { tt });
        }
    }

    public class ScheduleTeacher
    {
        public int StartTime { get; set; }
        public string ClassName { get; set; }
        public int Day { get; set; }
        public string TeacherId { get; set; }
    }

    public class TimeTableDetails
    {
        public int StartTime { get; set; }
        public int HoursPerDay { get; set; }
        public int PeriodDuration { get; set; }
        public int BreakDuration { get; set; }
        public int LabDuration { get; set; }
        public string? OrgId { get; set; }
        public string? Class { get; set; }
        public string? Division { get; set; }

        public int? Year { get; set; }

        public required List<Subject> Subjects { get; set; }
    }
}