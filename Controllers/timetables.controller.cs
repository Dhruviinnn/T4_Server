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

        [HttpGet("get/timetable")]
        public async Task<OkObjectResult> GetTimeTables()
        {
            var timetablesWholeData = await _timetableService.GetTimetableDataByOrgIdAsync(Request.Query["OrgId"].ToString());
            var timetables = timetablesWholeData.Select(tts => tts.Timetable);
            return Ok(new { timetables });
        }
    }
}
