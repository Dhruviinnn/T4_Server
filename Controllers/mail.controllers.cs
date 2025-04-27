using AuthString;
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



        [HttpPost("hello-get")]
        public async Task<IActionResult> helloGet()
        {
            return Ok(new { response = "Hello from Container : Docker" });
        }

        [HttpPost("decline")]
        public async Task<IActionResult> decline()
        {
            ApprovalDecline.Mail();
            return Ok(new { id = 'f' });
        }
        [HttpPost("user/forgot/mail")]
        public async Task<IActionResult> forgetpass(ChangePassword chg)
        {
            Forgetpass.Mail(chg.Email, new Authentication().Encode(chg.Email));
            return Ok(new { result = "Mail is sent, Check your Inbox" });
        }


        [HttpPost("approave")]
        public async Task<IActionResult> approave()
        {
            ApprovalSuccess.Mail("NextGen Academy", "vasavadhruvin123@gmail.com");
            return Ok(new { id = 'f' });
        }

        [HttpPost("send/mail")]
        public async Task<IActionResult> Authx()
        {
            Auth.Mail(["ORG82469235952", "Web University"]);
            return Ok(new { id = 'f' });
        }
    }
}