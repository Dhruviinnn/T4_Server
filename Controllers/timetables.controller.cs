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
        private List<List<Period>> generatedTT;
        string[] days;

        public TimetablesController(TimetableService timetableService, UserService userService)
        {
            _timetableService = timetableService;
            _userService = userService;
            generatedTT = new List<List<Period>>();
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
        public async Task<OkObjectResult> GetTimeTable([FromBody] TimetableData TimeTable)
        {
            List<Subject> subjects = TimeTable.Subjects;
            List<List<Period>> tt = new List<List<Period>>();

            int HoursPerDayInMinutes = (TimeTable.HoursPerDay * 60) - TimeTable.BreakDuration;
            Random rand = new Random();
            Dictionary<string, HashSet<string>> teacherSchedule = new Dictionary<string, HashSet<string>>();
            HashSet<string> isAllTeacherScheduled;
            HashSet<string> unavailableTeachers;
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
                                // add schedule to each teacher scheduleList
                                bool result = await _userService.AddScheduleToTeacher(teacher.TeacherId, new Schedule
                                {
                                    StartTime = currentStartTime,
                                    ClassName = TimeTable.Class,
                                    Day = i
                                });
                                if (result)
                                {
                                    tt[i].Add(new Period { StartTime = currentStartTime, Subject = subject });
                                    unavailableTeachers.Remove(subject.Teacher.TeacherId);
                                    currentStartTime += subject.IsLab ? TimeTable.LabDuration : TimeTable.PeriodDuration;
                                    teacherSchedule[day].Add(teacher.TeacherId);
                                }

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
                    printEachDay(tt[i], i, TimeTable);
                }
            }
            return Ok(new
            {
                GeneratedTimeTable = tt
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


        private void printEachDay(List<Period> day, int index, TimetableData TimeTable)
        {
            Console.WriteLine($"--{days[index]} : ");
            foreach (var item in day)
            {
                if (item.Subject.IsLab)
                {
                    Console.WriteLine($"{item.StartTime} : {item.StartTime + TimeTable.LabDuration}  -  {item.Subject.Name}  -  {item.Subject.Teacher.Name}");

                }
                else
                {
                    Console.WriteLine($"{item.StartTime} : {item.StartTime + TimeTable.PeriodDuration}  -  {item.Subject.Name}  -  {item.Subject.Teacher.Name}");
                }
            }
            Console.WriteLine("------------------------------------------------------------------");
        }
    }
}