using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cms;
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

        string[] days;

        public TimetablesController(TimetableService timetableService, UserService userService)
        {
            _timetableService = timetableService;
            _userService = userService;
            days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
        }



        [HttpGet("get/timetable-metadata")]
        public async Task<OkObjectResult> GetTimeTables()
        {
            var timetablesWholeData = await _timetableService.GetTimetableDataByOrgIdAsync(Request.Query["OrgId"].ToString());
            var timetables = timetablesWholeData.Select(tt => Ok(new
            {
                id = tt.Id,
                className = tt.Class,
                year = tt.Year,

            })).ToList();
            return Ok(new { timetables });
        }
        [HttpGet("delete/timetable")]
        public async Task<OkObjectResult> DeleteTimeTables()
        {
            await _timetableService.DeleteTimetableAsync(Request.Query["id"].ToString());
            return Ok(new { error = false, result = "TimeTable Deleted" });
        }

        private async Task<Dictionary<string, List<Schedule>>> GetScheduleListForAllTeachers(List<Teacher> teachers)
        {
            foreach (var item in teachers)
            {
            }
            Dictionary<string, List<Schedule>> x = new Dictionary<string, List<Schedule>>();
            foreach (var item in teachers)
            {
                var tmp = (await _userService.GetTeacherScheduleListAsync(item.TeacherId)).Schedule;
                x.Add(item.TeacherId, tmp ?? new List<Schedule>());
            }
            return x;
        }

        [HttpGet("get/timetable")]
        public async Task<OkObjectResult> GetTimeTableForSTU()
        {
            string className = Request.Query["class"].ToString();
            string orgId = Request.Query["orgId"].ToString();
            var tt = await _timetableService.GetTimetableAsync(className, orgId);
            return Ok(new { timetable = tt });
        }

        [HttpPost("upload/timetable")]
        public async Task<OkObjectResult> UploadTimeTable(TTUpload TT)
        {

            await _timetableService.InsertTimetableDataAsync(TT.TimeTable);

            foreach (var item in TT.ScheduledTeachers)
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
            return Ok(new { status = 200, result = "TimeTable Created" });
        }


        [HttpPost("generate/timetable")]
        public async Task<OkObjectResult> TmpTimeTable([FromBody] TimeTableDetails TimeTable)
        {
            List<Subject> subjects = TimeTable.Subjects;
            List<List<Period>> tt = Enumerable.Range(0, days.Length).Select(_ => new List<Period>()).ToList();

            int? HoursPerDayInMinutes = (TimeTable.HoursPerDay * 60) - TimeTable.BreakDuration;
            Random rand = new Random();
            List<Schedule> scheduledTeachers = new List<Schedule>();


            if (!(HoursPerDayInMinutes <= subjects.Count * TimeTable.PeriodDuration))
            {
                return Ok(new { status = 400, message = "Decrease hoursPerDay or Increase periodDurations or Increase subjects", GeneratedTimeTable = new List<int>() });
            }

            // Preprocess teacher schedule
            var scheduleListForTeachers = await GetScheduleListForAllTeachers(
                subjects.Select(s => s.Teacher).DistinctBy(t => t.TeacherId).ToList()
            );

            // Build a fast lookup: (teacherId, day, time) => unavailable
            var busySlots = new HashSet<(string TeacherId, int Day, int Time)>();

            foreach (var kv in scheduleListForTeachers)
            {
                foreach (var sched in kv.Value)
                {
                    for (int t = sched.StartTime; t < sched.StartTime + sched.Duration; t += TimeTable.PeriodDuration)
                    {
                        busySlots.Add((kv.Key, sched.Day, t));
                    }
                }
            }

            for (int dayIdx = 0; dayIdx < days.Length; dayIdx++)
            {
                string day = days[dayIdx];
                int currentTime = TimeTable.StartTime;
                var assignedTeachersToday = new HashSet<string>();

                while (currentTime - TimeTable.StartTime < HoursPerDayInMinutes)
                {
                    if (currentTime >= TimeTable.BreakStartTime && currentTime < TimeTable.BreakStartTime + TimeTable.BreakDuration)
                    {
                        // Schedule in break permitted
                        currentTime += TimeTable.BreakDuration!.Value;
                        continue;
                    }

                    // Get list of available subjects whose teachers are free now
                    var availableSubjects = subjects.Where(sub =>
                        !assignedTeachersToday.Contains(sub.Teacher.TeacherId) &&
                        !busySlots.Contains((sub.Teacher.TeacherId, dayIdx, currentTime))
                    ).ToList();

                    if (!availableSubjects.Any())
                    {
                        break; // No teacher free for this period
                    }

                    // Randomly pick from available
                    var selectedSubject = availableSubjects[rand.Next(availableSubjects.Count)];

                    tt[dayIdx].Add(new Period { StartTime = currentTime, Subject = selectedSubject });

                    scheduledTeachers.Add(new Schedule
                    {
                        StartTime = currentTime,
                        ClassName = TimeTable.Class,
                        Day = dayIdx,
                        TeacherId = selectedSubject.Teacher.TeacherId,
                        Subject = selectedSubject.Name,
                        IsLab = selectedSubject.IsLab,
                        Duration = selectedSubject.IsLab ? TimeTable.LabDuration : TimeTable.PeriodDuration
                    });

                    assignedTeachersToday.Add(selectedSubject.Teacher.TeacherId);
                    currentTime += selectedSubject.IsLab ? TimeTable.LabDuration : TimeTable.PeriodDuration;
                }
            }

            TimetableData generatedTT = new TimetableData
            {
                Timetable = tt,
                OrgId = TimeTable.OrgId,
                Class = TimeTable.Class,
                Year = TimeTable.Year,
                BreakStartTime = TimeTable.BreakStartTime,
                BreakDuration = TimeTable.BreakDuration,
                PeriodDuration = TimeTable.PeriodDuration,
                LabDuration = TimeTable.LabDuration
            };
            return Ok(new { status = 200, generatedTT, scheduledTeachers });
        }

    }

}


public class TimeTableDetails : TimetableData
{
    public int StartTime { get; set; }
    public int HoursPerDay { get; set; }
    public required List<Subject> Subjects { get; set; }
}
public class TTUpload
{
    public TimetableData TimeTable { get; set; }
    public List<Schedule> ScheduledTeachers { get; set; }
}

