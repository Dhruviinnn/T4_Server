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

namespace TimeFourthe.Controllers
{
    public class EmailRequest
    {
        public string Email { get; set; }
    }
    [Route("api")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

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
                    object userdata = new { userId = userExist.UserId, name = userExist.Name, email = userExist.Email, role = userExist.Role };
                    Response.Cookies.Append("auth", new Authentication().Encode(userdata));
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
                            email = userExist.Email
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
            var filteredTeacherlist = teacherlist.Select(teacher => new { id = teacher.Id, name = teacher.Name });
            return Ok(filteredTeacherlist);
        }

        [HttpGet("user/get")]
        public async Task<IActionResult> GetUser([FromBody] EmailRequest body)
        {
            var auth = Request.Cookies["auth"];
            if (auth != null) return Ok(new { user = new Authentication().Decode(auth) });
            return Ok(new { error = true, message = "Authorization failed" });
        }

        [HttpGet("get/students/email")]
        public async Task<IActionResult> GetStudents()
        {
            List<User> studentlist = await _userService.GetStudentsByOrgIdAsync(Request.Query["OrgId"].ToString());
            var filteredStudentsEmaillist = studentlist.Select(student => student.Email);
            return Ok(new { filteredStudentsEmaillist });
        }
    }
}