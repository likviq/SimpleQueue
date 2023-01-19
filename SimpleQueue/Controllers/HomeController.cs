using Microsoft.AspNetCore.Mvc;
using SimpleQueue.Models;
using System.Diagnostics;
using SimpleQueue.Domain.Interfaces;
using AutoMapper;
using SimpleQueue.Domain.Entities;
using SimpleQueue.WebUI.Models.DataTransferObjects;

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

        [HttpPost]
        public IActionResult Index([FromBody] CreateQueueDto queueForCreationDto)
        {
            var queue = _mapper.Map<Queue>(queueForCreationDto);
            _logger.LogInfo("asdasd");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error404()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}