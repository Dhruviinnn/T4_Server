using Microsoft.AspNetCore.Mvc;
using TimeFourthe.Entities;
using TimeFourthe.Services;
using IdGenerator;
using TimeFourthe.Mails;
using System.Text.Json;
using System.Text;
using AuthString;
using MongoDB.Bson;

namespace TimeFourthe.Controllers
{
    [Route("api")]
    [ApiController]
    public class PendingUsersContoller(PendingUserService pendingUserService, UserService userService) : ControllerBase
    {
        private readonly PendingUserService _pendingUserService = pendingUserService;
        private readonly UserService _userService = userService;

        [HttpPost("user/signup")]
        public async Task<IActionResult> CreatePendingUser([FromBody] User user)
        {
            if (user.Role != "organization")
            {
                User orgExist = await _userService.GetOrganizationByOrgId(user.OrgId);
                if (orgExist == null) return Ok(new { error = true, message = "This Organization is not exists" });
                User userExist = await _userService.GetUserAsync(user.Email);
                if (userExist != null) return Ok(new { error = true, message = "User already exists" });
                Console.WriteLine("Sign up for Teacher/Student");
                try
                {
                    await _userService.CreateUserAsync(user);
                    Response.Cookies.Append("auth", new Authentication().Encode(
                        new
                        {
                            userId = user.UserId,
                            name = user.Name,
                            email = user.Email,
                            role = user.Role
                        }
                    ));
                }
                catch (System.Exception)
                {

                    throw;
                }
                return Ok(new
                {
                    message = "User created successfully",
                    userData = new
                    {
                        name = user.Name,
                        userId = user.UserId,
                        role = user.Role,
                        email = user.Email
                    }
                });
            }
            else
            {
                var pendingUserExist = await _pendingUserService.GetPendingUserAsync(user.Email);
                if (pendingUserExist != null) return Ok(new { error = true, message = "Your request has not been approved yet " });
                Console.WriteLine("Sign up for Organization");
                List<string> org = await _pendingUserService.CreatePendingUserAsync(user);
                Auth.Mail(org);
                return Ok(new
                {
                    id = user.Name,
                    userData = new
                    {
                        name = user.Name,
                        userId = user.UserId,
                        role = user.Role,
                        email = user.Email
                    }
                });
            }
        }


        // /get/auth?id={orgId}&answer=true
        [HttpGet("get/auth")]
        public async Task<IActionResult> GetAuth()
        {
            string orgId = Request.Query["id"].ToString();
            string approve = Request.Query["answer"].ToString();
            var deletedUser = await _pendingUserService.DeletePendingUserAsync(orgId);
            if (approve == "true")
            {
                HttpClient client = new HttpClient();
                string json = JsonSerializer.Serialize(deletedUser);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("http://localhost:3000/api/user/create", content);
                ApprovalSuccess.Mail(deletedUser.Name, deletedUser.Email);
                return Ok(new { message = "Your application is approved by autority", response });
            }
            else
            {
                return Ok(new { message = "Your application not approved by autority" });
            }
        }

    }
}