using Microsoft.AspNetCore.Hosting;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Authentication.Cookies;

using SimpleNetCoreSecurityWeb.Services.Extensions;




var builder = WebApplication.CreateBuilder(args);

/*
https://dotnettutorials.net/lesson/difference-between-addmvc-and-addmvccore-method/
Adding web feature:

This is depending on which type of application you want to create.

1. If you want to create a Web API application where there are no views, then you need to use AddControllers() extension method.
2. If you want to work with the Razor Page application, then you need to use the AddRazorPages() extension method into your ConfigureService method of Startup class.
3. If you want to develop a Model View Controller i.e. MVC application then you need to use AddControllersWithViews() method.Further, if you want Pages features into your MVC application, then you need to use the AddMVC method.

Note:
a. AddMvc: This method has all the features. So, you can any type of application (Web API, MVC, and Razor Pages) using this AddMVC method.
b. Adding AddMvc() method will add extra features even though which are not required to your application which might impact the performance of the application.
                                                                                                                                                                                             
*/
builder.Services.AddControllersWithViews();

//custom service
builder.Services.RegisterAppServices();

//securiy
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Account/Login";
        option.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

//security
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action}/{id?}"
    );

//app.MapGet("/", () => "Hello World!");

app.Run();