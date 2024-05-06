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
//options.Password = redisPassword;      


builder.Services.AddDataProtection()
    .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(options),"DataProtection-Keys")
    .SetApplicationName("neutro");

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o=>
    {
        o.Cookie.Domain = ".company.local";
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/", () => "Hello app1");

app.MapGet("/protected", () => "secret app 1!")
    .RequireAuthorization();

app.Run();
