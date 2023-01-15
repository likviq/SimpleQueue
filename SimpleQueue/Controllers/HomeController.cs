using Microsoft.AspNetCore.Mvc;
using SimpleQueue.Models;
using System.Diagnostics;
using SimpleQueue.Domain.Interfaces;

namespace SimpleQueue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILoggerManager _logger;

        public HomeController(ILoggerManager logger)
        {
            _logger = logger;
            _logger.LogDebug("NLog injected into HomeController");
        }

        public IActionResult Index()
        {
            _logger.LogInfo("asdasd");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}