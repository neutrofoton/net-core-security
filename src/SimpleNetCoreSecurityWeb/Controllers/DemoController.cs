using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SimpleNetCoreSecurityWeb.Controllers
{
    [Route("demo")]
    public class DemoController : Controller
    {
        [Route("index")]
        [Route("")]
        [Route("~/")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        [Route("index2")]
        public IActionResult Index2()
        {
            return View("Index2");
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [Route("index3")]
        public IActionResult Index3()
        {
            return View("Index3");
        }

        [Authorize(Roles = "SuperAdmin,Admin,Employee")]
        [Route("index4")]
        public IActionResult Index4()
        {
            return View("Index4");
        }
    }
}
