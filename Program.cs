// using IdGenerator;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TimeFourthe.Configurations;
using TimeFourthe.Services;

// For JWT
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
// For JWT

var key = Encoding.UTF8.GetBytes("1234");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowCredentials().AllowAnyMethod().AllowAnyHeader();
    });
});

// MongoDB Settings
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<TimetableService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<PendingUserService>();

// Controllers
builder.Services.AddControllers();
var app = builder.Build();

app.UseCors("AllowFrontend");
app.MapControllers();
app.Run();