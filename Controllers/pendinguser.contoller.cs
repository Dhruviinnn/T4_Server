using Microsoft.AspNetCore.Mvc;
using TimeFourthe.Entities;
using TimeFourthe.Services;
using IdGenerator;
using TimeFourthe.Mails;
using System.Text.Json;
using System.Text;
using MongoDB.Bson;

namespace TimeFourthe.Controllers
{
    [Route("api")]
    [ApiController]
    public class PendingUsersContoller(PendingUserService pendingUserService) : ControllerBase
    {
        private readonly PendingUserService _pendingUserService = pendingUserService;

        [HttpPost("create/pending")]
        public async Task<IActionResult> CreatePendingUser([FromBody] User user)
        {
            List<string> org = await _pendingUserService.CreatePendingUserAsync(user);
            Auth.MailOtp(org);
            return Ok(new { id = user.Name });
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
                HttpResponseMessage response = await client.PostAsync("http://localhost:5140/api/create", content);

                ApprovalSuccess.MailOtp(deletedUser.Name,deletedUser.Email);
                return Ok(new { message = "Your application is approved by autority", response });
            }
            else
            {
                return Ok(new { message = "Your application not approved by autority" });
            }
            // responce from server
            // if yes then move user in User from pending table
        }

    }
}