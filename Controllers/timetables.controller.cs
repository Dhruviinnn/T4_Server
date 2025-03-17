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

        public TimetablesController(TimetableService timetableService)
        {
            _timetableService = timetableService;
        }

        [HttpPost("create-timetable")]
        public async Task<OkObjectResult> CreateTimetable([FromBody] TimetableData timetableData)
        {
            if (timetableData==null || !timetableData.Subjects.Any()) return Ok(new { status=400,error="Error while Timetable data fetching" });

            try
            {
                timetableData.Timetable=GenerateTimetable(timetableData);
                await _timetableService.InsertTimetableDataAsync(timetableData);
                // return Ok(new { timetable=timetableData.Timetable});
                 return Ok(new { success="TimeTable Created Successfully" });
                // return Ok(new { id = timetableData.tableId, message = "Timetable created successfully." });
            }
            catch (Exception ex){return Ok(new { status=500,error = "An error occurred while creating the timetable" });}
        }



          private Timetable GenerateTimetable(TimetableData timetableData) {
            if (timetableData == null)
                throw new ArgumentNullException(nameof(timetableData), "Timetable data cannot be null");

            if (timetableData.Subjects == null || !timetableData.Subjects.Any()) 
                throw new ArgumentException("Subjects list cannot be null or empty", nameof(timetableData.Subjects));

            var timetable = new Timetable {
                Monday = new List<Period>(),
                Tuesday = new List<Period>(),
                Wednesday = new List<Period>(),
                Thursday = new List<Period>(),
                Friday = new List<Period>(),
                Saturday = new List<Period>()
            };

            var days = new List<List<Period>> {
                timetable.Monday,
                timetable.Tuesday,
                timetable.Wednesday,
                timetable.Thursday,
                timetable.Friday,
                timetable.Saturday
            };

            var subjects = timetableData.Subjects;
            var random = new Random();
            var teacherSchedules = new Dictionary<string, HashSet<int>>();

            foreach (var subject in subjects) {
                if (subject.Teacher == null || string.IsNullOrEmpty(subject.Teacher.Id))
                    throw new ArgumentException("Each subject must have a valid teacher assigned.", nameof(subject.Teacher));

                if (!teacherSchedules.ContainsKey(subject.Teacher.Id))
                    teacherSchedules[subject.Teacher.Id] = new HashSet<int>();

                int lecturesAssigned = 0;
                int maxAttempts = 100;

                while (lecturesAssigned < 4 && maxAttempts > 0) {
                    maxAttempts--;
                    int dayIndex = random.Next(0, days.Count);

                    if (!days[dayIndex].Any(p => p.Subject.Teacher?.Id == subject.Teacher.Id)) {
                        int startTime = GetAvailableTimeSlot(days[dayIndex], timetableData, subject.Teacher, teacherSchedules);

                        if (startTime != -1) {
                            var newPeriod = new Period {
                                StartTime = startTime,
                                Subject = subject,
                                IsLab = false
                            };

                            days[dayIndex].Add(newPeriod);
                            teacherSchedules[subject.Teacher.Id].Add(startTime);
                            lecturesAssigned++;
                        }
                    }
                }
            }

            return timetable;
        }

        private int GetAvailableTimeSlot(List<Period> periods, TimetableData timetableData, Teacher teacher, Dictionary<string, HashSet<int>> teacherSchedules) {
            int startTime = 8 * 60; // Start at 8:00 AM in minutes
            int endTime = startTime + (timetableData.HoursPerDay * 60);

            for (int time = startTime; time < endTime; time += timetableData.PeriodDuration + timetableData.BreakDuration) {
                if (!periods.Any(p => p.StartTime == time) && 
                    (!teacherSchedules.ContainsKey(teacher.Id) || !teacherSchedules[teacher.Id].Contains(time))) {
                    return time;
                }
            }
            return -1; // No available slot
        }
    }
}
