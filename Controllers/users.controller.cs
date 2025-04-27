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
        private readonly List<List<string>> classes = [
            ["Nursery", "Pre-Kindergarten", "Kindergarten"],
            ["Class I", "Class II", "Class III", "Class IV", "Class V"],
            ["Class VI", "Class VII", "Class VIII", "Class IX", "Class X", "Class XI", "Class XII"],
            ["1st Year", "2nd Year", "3th Year", "4th Year", "5th Year", "6th Year", "7th Year"]
           ];
        public UsersController(UserService userService)
        {
            _userService = userService;
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
                    object userdata = new { userId = userExist.UserId, name = userExist.Name, email = userExist.Email, role = userExist.Role, orgId = userExist.OrgId };
                    Response.Cookies.Append("auth", new Authentication().Encode(userdata), new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(7)
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
            Console.WriteLine($"Auth : {auth}");
            if (auth != null) return Ok(new { user = new Authentication().Decode(auth) });
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
            Absence.Mail(filteredStudentsEmaillist.ToArray(), absentData.Name, absentData.SubjectName, "Web Web Web");
            return Ok(new { filteredStudentsEmaillist });
        }

        [HttpGet("get/org/classes")]
        public OkObjectResult GetClasses()
        {

            string OrgId = Request.Query["OrgId"].ToString();
            List<int> orgType = [0, 1, 2, 3]; // fetch from db using orgId
            List<string> orgClasses = [];
            foreach (var item in orgType)
            {
                orgClasses = orgClasses.Concat(classes[item]).ToList();
            }
            return Ok(new { orgClasses });

        }


        [HttpPost("user/update/changepassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] ChangePassword chg)
        {
            chg.Email = new Authentication().Decode(chg.Id).ToString();
            bool result = await _userService.UpdateUserAsync(chg.Email, chg.NewPassword);
            return Ok(new { result });
        }
    }
}