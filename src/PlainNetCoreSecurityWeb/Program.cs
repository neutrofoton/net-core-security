using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;



// MODE 1: Checking claim inside method
/*
const string CookieAuthSchema = "cookie";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthSchema)
    .AddCookie(CookieAuthSchema);

var app = builder.Build();

app.UseAuthentication();


//map url pattern
app.MapGet("/", () => "Hello World!");
app.MapGet("/unsecure",(HttpContext ctx) =>
{
    return ctx.User.FindFirst("usr")?.Value ?? "empty";
});

app.MapGet("/secure", (HttpContext ctx) =>
{
    if(!ctx.User.Identities.Any(x => x.AuthenticationType== CookieAuthSchema))
    {
        ctx.Response.StatusCode = 401;
        return string.Empty;
    }

    if(!ctx.User.HasClaim("passport_type","eur"))
    {
        ctx.Response.StatusCode = 403;
        return string.Empty;
    }
    return "allowed";
});

app.MapGet("/login", async (HttpContext ctx) =>
{
    var claims = new List<Claim>();
    claims.Add(new Claim("usr", "admin"));
    claims.Add(new Claim("passport_type", "eur"));

    var identity = new ClaimsIdentity(claims, CookieAuthSchema);
    var user = new ClaimsPrincipal(identity);

    await ctx.SignInAsync(CookieAuthSchema, user);

});

app.Run();
*/



// MODE 2: Using Own Authorization Middleware
/*
const string CookieAuthSchema = "cookie";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthSchema)
    .AddCookie(CookieAuthSchema);

var app = builder.Build();

app.UseAuthentication();

//use own authorization middleware
app.Use((ctx, next) =>
{
    if (ctx.Request.Path.StartsWithSegments("/login") || ctx.Request.Path.StartsWithSegments("/unsecure"))
    {
        return next();
    }

    if (!ctx.User.Identities.Any(x => x.AuthenticationType == CookieAuthSchema))
    {
        ctx.Response.StatusCode = 401;
        return Task.CompletedTask;
    }

    if (!ctx.User.HasClaim("passport_type", "eur"))
    {
        ctx.Response.StatusCode = 403;
        return Task.CompletedTask;
    }

    return next();
});


//map url pattern
app.MapGet("/", () => "Hello World!");
app.MapGet("/unsecure",(HttpContext ctx) =>
{
    return ctx.User.FindFirst("usr")?.Value ?? "empty";
});

app.MapGet("/secure", (HttpContext ctx) =>
{
    
    return "allowed";
});

app.MapGet("/login", async (HttpContext ctx) =>
{
    var claims = new List<Claim>();
    claims.Add(new Claim("usr", "admin"));
    claims.Add(new Claim("passport_type", "eur"));

    var identity = new ClaimsIdentity(claims, CookieAuthSchema);
    var user = new ClaimsPrincipal(identity);

    await ctx.SignInAsync(CookieAuthSchema, user);

});

app.Run();

*/

// MODE 3: Using NetCore Authorization Middleware

const string CookieAuthSchema = "cookie";

var builder = WebApplication.CreateBuilder(args);

//Authentication configuration
builder.Services.AddAuthentication(CookieAuthSchema)
    .AddCookie(CookieAuthSchema);

//Authorization configuration
builder.Services.AddAuthorization(builder =>
{
    builder.AddPolicy("eur_passport", pb =>
    {
        pb
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(CookieAuthSchema)
        //.AddRequirements() // check documentation for detail
        .RequireClaim("passport_type", "eur")
        ;
          
    });
});


var app = builder.Build();

// Authentication Middleware
app.UseAuthentication();

// Authorization Middleware
app.UseAuthorization();

//map url pattern
app.MapGet("/", () => "Hello World!");
app.MapGet("/unsecure", (HttpContext ctx) =>
{
    return ctx.User.FindFirst("usr")?.Value ?? "empty";
});

//[Authorize(Policy = "eur_passport")] => for using Controller
app.MapGet("/secure", (HttpContext ctx) =>
{
    return "allowed";
}).RequireAuthorization("eur_passport");

app.MapGet("/login", async (HttpContext ctx) =>
{
    var claims = new List<Claim>();
    claims.Add(new Claim("usr", "admin"));
    claims.Add(new Claim("passport_type", "eur"));

    var identity = new ClaimsIdentity(claims, CookieAuthSchema);
    var user = new ClaimsPrincipal(identity);

    await ctx.SignInAsync(CookieAuthSchema, user);

}).AllowAnonymous();

app.Run();
