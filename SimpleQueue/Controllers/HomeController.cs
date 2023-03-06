using Microsoft.AspNetCore.Mvc;
using SimpleQueue.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Authentication;

namespace SimpleQueue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/login")]
        public IActionResult Login()
        {
            _logger.LogInformation($"Login operation started");
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/"
            });
        }

        [HttpGet("/register")]
        public IActionResult Register()
        {
            var callbackUrl = Url.Action("Register");

            var props = new AuthenticationProperties
            {
                RedirectUri = callbackUrl
            };

            return Challenge(props);
        }

        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            _logger.LogInformation($"Sign out operation started");
            return SignOut("Cookie", "oidc");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode, Exception ex)
        {
            var errorViewModel = new ErrorViewModel
            {
                StatusCode = statusCode,
                StatusDescription = ReasonPhrases.GetReasonPhrase(statusCode)
            };

            return View(errorViewModel);
        }
    }
}