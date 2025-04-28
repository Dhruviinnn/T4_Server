using Microsoft.AspNetCore.Mvc;
using TimeFourthe.Entities;
using TimeFourthe.Services;
using IdGenerator;
using TimeFourthe.Mails;
using System.Text.Json;
using System.Text;
using AuthString;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http;

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
            User userExist = await _userService.GetUserAsync(user.Email);
            if (userExist != null) return Ok(new { error = true, message = "User already exists" });
            if (user.Role != "organization")
            {
                User orgExist = await _userService.GetOrganizationByOrgId(user.OrgId);
                if (orgExist == null) return Ok(new { error = true, message = "This Organization is not exists" });
                try
                {
                    await _userService.CreateUserAsync(user);
                    Response.Cookies.Append("auth", new Authentication().EncodeJwt(
                        new
                        {
                            userId = user.UserId,
                            name = user.Name,
                            email = user.Email,
                            role = user.Role,
                            orgId = user.OrgId,
                            className = user.Class
                        }
                    ), new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(7)
                    });
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
                        email = user.Email,
                        orgId = user.OrgId,
                        className = user.Class
                    }
                });
            }
            else
            {
                var pendingUserExist = await _pendingUserService.GetPendingUserAsync(user.Email);
                if (pendingUserExist != null) return Ok(new { error = true, message = "Your request has not been approved yet " });
                List<string> org = await _pendingUserService.CreatePendingUserAsync(user);
                Auth.Mail(org);
                return Ok(new
                {
                    error = false,
                    message = "Logged In",
                    userData = new
                    {
                        name = user.Name,
                        userId = user.UserId,
                        role = user.Role,
                        email = user.Email,
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
                await _userService.CreateUserAsync(deletedUser);
                ApprovalSuccess.Mail(deletedUser.Name, deletedUser.Email);
                return Ok(new { message = "Your application is approved by autority" });
            }
            else
            {
                ApprovalDecline.Mail(deletedUser.Email);
                return Ok(new { message = "Your application not approved by autority" });
            }
        }

    }
}