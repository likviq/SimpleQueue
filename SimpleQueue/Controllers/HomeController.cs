using Microsoft.AspNetCore.Mvc;
using SimpleQueue.Models;
using SimpleQueue.Domain.Interfaces;
using AutoMapper;
using SimpleQueue.Domain.Entities;
using SimpleQueue.WebUI.Models.DataTransferObjects;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Authentication;

namespace SimpleQueue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public HomeController(ILoggerManager logger, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug("NLog injected into HomeController");
            _mapper = mapper;
        }

        [HttpGet("/login")]
        public IActionResult Login()
        {
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