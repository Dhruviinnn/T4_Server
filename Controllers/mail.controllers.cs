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
        private readonly UserService _userService;

        public MailController(TimetableService timetableService, UserService userService)
        {
            _timetableService = timetableService;
            _userService = userService;
        }

        [HttpPost("decline")]
        public async Task<IActionResult> decline()
        {
            ApprovalDecline.Mail("vasavadhruvin123@gmail.com");
            return Ok(new { id = 'f' });
        }
        [HttpPost("user/forgot/mail")]
        public async Task<IActionResult> forgetpass(ChangePassword chg)
        {
            var user = await _userService.GetUserAsync(chg.Email);
            if (user == null) return Ok(new { status = 400, result = "Given email is not associated with any account" });
            Forgetpass.Mail(chg.Email, new Authentication().EncodeJwt(chg.Email));
            return Ok(new { status = 200, result = "Mail is sent, Check your Inbox" });
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