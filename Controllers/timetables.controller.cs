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
        private List<Schedule> scheduledTeachers;
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

        [HttpGet("get/timetable-metadata")]
        public async Task<OkObjectResult> GetTimeTables()
        {
            var timetablesWholeData = await _timetableService.GetTimetableDataByOrgIdAsync(Request.Query["OrgId"].ToString());
            var timetables = timetablesWholeData.Select(tt => Ok(new
            {
                id = tt.Id,
                className = tt.Class,
                division = tt.Division,
                year = tt.Year,
                breakDuration = tt.BreakDuration,
                breakStartTime = tt.BreakStartTime,

            }));
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
            scheduledTeachers = new List<Schedule>();

            Dictionary<string, List<Schedule>> scheduleListForTeachers = await GetScheduleListForAllTeachers(TimeTable.Subjects.Select(sub => sub.Teacher).DistinctBy(teacher => teacher.TeacherId).ToList());
            for (int i = 0; i < days.Length; i++)
            {
                string day = days[i];
                int currentStartTime = TimeTable.StartTime;
                unavailableTeachers = new HashSet<string>();
                isAllTeacherScheduled = new HashSet<string>();
                if (!(HoursPerDayInMinutes <= subjects.Count * TimeTable.PeriodDuration))
                {
                    return Ok(new { status = 400, message = "Decrease hoursPerDay or Increase periodDuraions", GeneratedTimeTable = new List<int>() });
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
                                break;
                            }
                        }


                        if (isTeacherAvailable(scheduleListForTeachers[teacher.TeacherId], i, currentStartTime, teacher.TeacherId, TimeTable.PeriodDuration, isAllTeacherScheduled, unavailableTeachers, TimeTable.BreakStartTime, TimeTable.BreakDuration))
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

                                scheduledTeachers.Add(new Schedule
                                {
                                    StartTime = currentStartTime,
                                    ClassName = TimeTable.Class,
                                    Day = i,
                                    TeacherId = subject.Teacher.TeacherId,
                                    Subject = subject.Name,
                                    IsLab = subject.IsLab,
                                    Duration = subject.IsLab ? TimeTable.LabDuration : TimeTable.PeriodDuration
                                });

                            }
                            catch (System.Exception)
                            {
                                tt.Add(new List<Period>());
                            }
                        }
                        if (scheduleListForTeachers.Count == isAllTeacherScheduled.Count)
                        {
                            return Ok(new { status = 400, message = "All teachers are scheduled" });
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
                Year = TimeTable.Year,
                BreakStartTime = TimeTable.BreakStartTime,
                BreakDuration = TimeTable.BreakDuration,
                PeriodDuration = TimeTable.PeriodDuration,
                LabDuration = TimeTable.LabDuration
            };

            foreach (var item in scheduledTeachers)
            {
                Console.WriteLine($"{item.ClassName} -- {item.Day} -- {item.StartTime} -- {item.TeacherId}");
            }

            return Ok(new { status = 200, generatedTT });
        }

        private async Task<Dictionary<string, List<Schedule>>> GetScheduleListForAllTeachers(List<Teacher> teachers)
        {
            foreach (var item in teachers)
            {
                Console.WriteLine($"Schedule of {item.Name} - {item.TeacherId}");
            }
            Dictionary<string, List<Schedule>> x = new Dictionary<string, List<Schedule>>();
            foreach (var item in teachers)
            {
                var tmp = (await _userService.GetTeacherScheduleListAsync(item.TeacherId)).Schedule;
                x.Add(item.TeacherId, tmp ?? new List<Schedule>());
            }
            return x;
        }
        private bool isTeacherAvailable(List<Schedule> scheduleListForTeacher, int day, int time, string teacherId, int PeriodDuration, HashSet<String> isAllTeacherScheduled, HashSet<String> unavailableTeachers, int? breakStartTime, int? breakDuration)
        {

            if (time >= breakStartTime && time < breakStartTime + breakDuration) return false;
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
                await _userService.AddScheduleToTeacher(item.TeacherId, new Schedule
                {
                    StartTime = item.StartTime,
                    Day = item.Day,
                    ClassName = item.ClassName,
                    Subject = item.Subject,
                    IsLab = item.IsLab,
                    Duration = item.Duration
                });
            }
            return Ok(new { result = "TimeTable Created" });
        }



        [HttpPost("show/tt")]
        public async Task<OkObjectResult> DeleteTimeTable()
        {
            var tt = await _timetableService.GetTimetableAsync("67dcf92e2318d3633ffcbb85");
            return Ok(new { tt });
        }
    }


    public class TimeTableDetails : TimetableData
    {
        public int StartTime { get; set; }
        public int HoursPerDay { get; set; }
        public required List<Subject> Subjects { get; set; }
    }
}