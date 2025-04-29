using Microsoft.AspNetCore.Mvc;
using TimeFourthe.Entities;
using TimeFourthe.Services;
using IdGenerator;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
using System.Text;
using AuthString;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using TimeFourthe.Mails;
using System.Threading.Tasks;

namespace TimeFourthe.Controllers
{
    public class AbsentDataRequest
    {
        public string Class { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public string SubjectName { get; set; }
        public string OrgId { get; set; }
    }
    public class ChangePassword
    {
        public string Email { get; set; }
        public string? NewPassword { get; set; }
        public string? Id { get; set; }
    }
    [Route("api")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TimetableService _ttService;
        private readonly List<List<string>> classes = [
            ["Nursery", "Pre-Kindergarten", "Kindergarten"],
            ["Class I", "Class II", "Class III", "Class IV", "Class V"],
            ["Class VI", "Class VII", "Class VIII", "Class IX", "Class X", "Class XI", "Class XII"],
            ["1st Year", "2nd Year", "3th Year", "4th Year", "5th Year", "6th Year", "7th Year"]
           ];
        public UsersController(UserService userService, TimetableService ttService)
        {
            _userService = userService;
            _ttService = ttService;
        }

        // For login
        [HttpPost("user/login")]
        public async Task<IActionResult> GetUsers([FromBody] User user)
        {
            var userExist = await _userService.GetUserAsync(user.Email);
            if (userExist != null)
            {
                if (userExist.Password == user.Password)
                {
                    object userdata = new { userId = userExist.UserId, name = userExist.Name, email = userExist.Email, role = userExist.Role, orgId = userExist.OrgId, className = userExist.Class };
                    Response.Cookies.Append("auth", new Authentication().EncodeJwt(userdata), new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(7),
                    });
                    return Ok(new
                    {
                        error = false,
                        redirectUrl = "/timetable",
                        message = "Succesfully Login",
                        userData = new
                        {
                            name = userExist.Name,
                            userId = userExist.UserId,
                            role = userExist.Role,
                            email = userExist.Email,
                            orgId = userExist.OrgId,
                            className = userExist.Class
                        }
                    });
                }
                else
                {
                    return Ok(new { error = true, redirectUrl = "/login", message = "Password Incorrect" });
                }
            }
            return Ok(new { error = true, redirectUrl = "/login", message = "User not exists", data = userExist });
        }

        [HttpGet("get/teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            List<User> teacherlist = await _userService.GetTechersByOrgIdAsync(Request.Query["OrgId"].ToString());
            var filteredTeacherlist = teacherlist.Select(teacher => new { userId = teacher.UserId, name = teacher.Name });
            return Ok(filteredTeacherlist);
        }

        [HttpGet("user/get")]
        public OkObjectResult GetUser()
        {
            var auth = Request.Cookies["auth"];
            if (auth != null) return Ok(new { user = new Authentication().DecodeJwt(auth) });
            return Ok(new { error = true, message = "Authorization failed" });
        }
        [HttpGet("user/logout")]
        public OkObjectResult LogOut()
        {
            var auth = Request.Cookies["auth"];
            if (auth != null) Response.Cookies.Delete("auth");
            return Ok(new { status = true });
        }

        [HttpPost("user/teacher/absent")]
        public async Task<IActionResult> GetStudents([FromBody] AbsentDataRequest absentData)
        {
            List<User> studentlist = await _userService.GetStudentsByOrgIdAndClassAsync(absentData);
            var filteredStudentsEmaillist = studentlist.Select(student => student.Email);
            string OrgName = (await _userService.GetOrgNameByOrgId(absentData.OrgId)).Name;
            Absence.Mail(filteredStudentsEmaillist.ToArray(), absentData.Name, absentData.SubjectName, OrgName, absentData.Date);
            return Ok(new { status = 200, message = "Mail sent to all students" });
        }


        [HttpGet("get/org/classes")]
        public async Task<OkObjectResult> GetClasses()
        {

            string OrgId = Request.Query["OrgId"].ToString();
            var orgType = (await _userService.GetClassesByOrgId(OrgId)).OrgType;
            List<string> orgClasses = [];
            if (orgType != null)
            {
                foreach (var item in orgType)
                {
                    orgClasses = orgClasses.Concat(classes[item]).ToList();
                }
            }
            return Ok(new { orgClasses });

        }


        [HttpPost("user/update/changepassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] ChangePassword chg)
        {
            chg.Email = new Authentication().DecodeJwt(chg.Id).ToString();
            bool result = await _userService.UpdateUserAsync(chg.Email, chg.NewPassword);
            return Ok(new { result });
        }

        [HttpGet("get/schedule")]
        public async Task<OkObjectResult> GetTeacherSchedules()
        {

            string TeacherId = Request.Query["TeacherId"].ToString();
            var Schedule = (await _userService.GetTeacherScheduleListAsync(TeacherId)).Schedule;
            return Ok(new { Schedule });

        }
    }
}