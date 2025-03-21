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

        public TimetablesController(TimetableService timetableService, UserService userService)
        {
            _timetableService = timetableService;
            _userService = userService;
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
        public async Task<OkObjectResult> GetTimeTable([FromBody] TimetableData TimeTable)
        {
            List<Subject> subjects = TimeTable.Subjects;
            string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            List<List<Period>> tt = new List<List<Period>>();

            int[] periodsPerDay = { 5, 5, 5, 5, 5, 5 }; // Generate, this is hard one
            int HoursPerDayInMinutes = (TimeTable.HoursPerDay * 60) - TimeTable.BreakDuration;

            Random rand = new Random();
            Dictionary<string, HashSet<string>> teacherSchedule = new Dictionary<string, HashSet<string>>();

            Dictionary<string, List<Schedule>> scheduleListForTeachers = await GetScheduleListForAllTeachers();

            for (int i = 0; i < days.Length; i++)
            {
                string day = days[i];
                int periods = periodsPerDay[i];
                int currentStartTime = TimeTable.StartTime;

                if (!(HoursPerDayInMinutes <= periods * TimeTable.PeriodDuration))
                {
                    return Ok(new { error = "Decrease hoursPerDay or Increase periodDuraions", GeneratedTimeTable = new List<int>() });
                }
                else
                {
                    for (int j = 0; (currentStartTime - TimeTable.StartTime) <= HoursPerDayInMinutes; j++)
                    {
                        Subject subject;
                        Teacher teacher;

                        do
                        {
                            subject = subjects[rand.Next(subjects.Count)];
                            teacher = subject.Teacher;
                        } while (teacherSchedule.ContainsKey(day) && teacherSchedule[day].Contains(teacher.TeacherId));

                        if (isTeacherAvailable(scheduleListForTeachers["TCH379477830408"], i, currentStartTime, teacher.TeacherId, TimeTable.PeriodDuration)) // instead of hard code teacher userId, give value as list
                        {
                            if (!teacherSchedule.ContainsKey(day))
                            {
                                teacherSchedule[day] = new HashSet<string>();
                            }
                            teacherSchedule[day].Add(teacher.TeacherId);

                            try
                            {
                                tt[i].Add(new Period { StartTime = currentStartTime, Subject = subject });
                            }
                            catch (System.Exception)
                            {
                                tt.Add(new List<Period>());
                                // throw;
                            }
                            currentStartTime += TimeTable.PeriodDuration;
                        }
                    }
                }
            }
            return Ok(new { GeneratedTimeTable = tt });
        }

[HttpGet("getch")]
        public async Task<Dictionary<string, List<Schedule>>> GetScheduleListForAllTeachers()
        {
            List<string> teacherIds = ["TCH379477830408"];
            Dictionary<string, List<Schedule>> x = new Dictionary<string, List<Schedule>>();
            foreach (var item in teacherIds)
            {
                x.Add(item, (await _userService.GetTeacherScheduleListAsync(item)).Schedule);
            }
            return x;
        }
        private bool isTeacherAvailable(List<Schedule> scheduleListForTeacher, int day, int time, string teacherId, int PeriodDuration)
        {
            var tmp = scheduleListForTeacher.Find(item => item.Day == day && time >= item.StartTime && time <= item.StartTime + PeriodDuration);
            return tmp == null;
        }

    }
}
