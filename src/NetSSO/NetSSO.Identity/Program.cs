using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

//add data protection
var redisAddress = builder.Configuration.GetValue<string>("Redis:IpPort");
var redisPassword = builder.Configuration.GetValue<string>("Redis:Password");

var options = ConfigurationOptions.Parse(redisAddress!); // host1:port1, host2:port2, ...
options.Password = redisPassword;      

builder.Services.AddDataProtection()
    .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(options))
    .SetApplicationName("neutro");

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/", () => "Hello identity");

app.MapGet("/login",(HttpContext ctx) =>
{
    ctx.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity[]{
        new ClaimsIdentity(new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString())
        }, CookieAuthenticationDefaults.AuthenticationScheme)
    }));

    return "ok";
});

app.MapGet("/protected", () => "secret!")
    .RequireAuthorization();

app.Run();
