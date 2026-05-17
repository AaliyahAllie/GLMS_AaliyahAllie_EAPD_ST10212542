using Microsoft.AspNetCore.Mvc;

namespace GLMS.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var adminUsername = _configuration["AdminLogin:Username"];
            var adminPassword = _configuration["AdminLogin:Password"];

            if (username == adminUsername && password == adminPassword)
            {
                HttpContext.Session.SetString("AdminLoggedIn", "true");
                HttpContext.Session.SetString("AdminUsername", username);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid admin username or password.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Public");
        }
    }
}