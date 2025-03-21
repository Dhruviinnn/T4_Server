using Microsoft.AspNetCore.Mvc;
using TimeFourthe.Entities;
using TimeFourthe.Mails;
using TimeFourthe.Services;

namespace TimeFourthe.Controllers
{
    [Route("api")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly TimetableService _timetableService;
        public MailController(TimetableService timetableService)
        {
            _timetableService = timetableService;
        }

        [HttpPost("send-auth")]
        public async Task<IActionResult> Authx()
        {
            Auth.Mail(["ORG82469235952", "Web University"]);
            return Ok(new { id = 'f' });
        }
    }
}