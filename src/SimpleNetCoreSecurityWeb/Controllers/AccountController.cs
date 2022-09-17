using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SimpleNetCoreSecurityWeb.Services;
using System.Security.Claims;

namespace SimpleNetCoreSecurityWeb.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var account = accountService.Login(username, password);

            if(account == null)
            {
                ViewBag.Message = "Invalid Username or Password";

                return View("Login");
            }
            else
            {
                IEnumerable<Claim> accountClaims = accountService.GetClaimsAccount(account);

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(accountClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                return RedirectToAction("welcome");
            }

            
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index","Demo");
        }

        [Route("welcome")]
        public IActionResult Welcome()
        {
            var account = User.FindFirst(ClaimTypes.Name);
            ViewBag.Username = account.Value;

            return View("Welcome");
        }

        [Route("accessDenied")]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}
