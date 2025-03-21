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

       

         [HttpPost("absent")]
        public async Task<IActionResult> absent()
        {
            Absence.Mail();
            return Ok(new { id = 'f' });
        }
         [HttpPost("decline")]
        public async Task<IActionResult> decline()
        {
            ApprovalDecline.Mail();
            return Ok(new { id = 'f' });
        }
         [HttpPost("forget")]
        public async Task<IActionResult> forgetpass()
        {
            Forgetpass.Mail();
            return Ok(new { id = 'f' });
        }


         [HttpPost("approave")]
        public async Task<IActionResult> approave()
        {
            ApprovalSuccess.Mail("NextGen Academy","vasavadhruvin123@gmail.com");
            return Ok(new { id = 'f' });
        }

        [HttpPost("send-auth")]
        public async Task<IActionResult> Authx()
        {
            Auth.Mail(["ORG82469235952", "Web University"]);
            return Ok(new { id = 'f' });
        }
    }
}