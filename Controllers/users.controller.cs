using Microsoft.AspNetCore.Mvc;
using TimeFourthe.Entities;
using TimeFourthe.Services;
using IdGenerator;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

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

                    // Response.Cookies.Append("auth", userExist.UserId);
                    return Ok(new { error = false, redirectUrl = "/timetable", message = "Succesfully Login" });
                }
                else
                {
                    return Ok(new { error = true, redirectUrl = "/login", message = "Password Incorrect" });
                }
            }
            return Ok(new { error = true, redirectUrl = "/login", message = "User not exists", data = userExist });
        }

        // for signup
        [HttpPost("user/create")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var userExist = await _userService.GetUserAsync(user.Email);
            await _userService.CreateUserAsync(user);
            return Ok(new { error = false, redirectUrl = "/timetable", message = "User created successfully" });
        }

        // get teachers by OrgId
        [HttpGet("teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            var teacherlist = await _userService.GetTechersByOrgIdAsync(Request.Query["OrgId"].ToString());
            return Ok(teacherlist);
        }

        [HttpPost("user/get")]
        public async Task<IActionResult> GetUser([FromBody] EmailRequest body)
        {
            var userExist = await _userService.GetUserAsync(body.Email);
            return Ok(new { user = userExist });
        }
    }
}