using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleQueue.Data;
using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.WebUI.Models.DataTransferObjects;

namespace SimpleQueue.WebUI.Controllers
{
    public class QueueController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public QueueController(IMapper mapper, IRepositoryManager repository, ILoggerManager logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateQueueDto? createQueueDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("createQueueDto object is null");
                    return BadRequest();
                }

                var queue = _mapper.Map<Queue>(createQueueDto);

                _repository.Queue.CreateQueue(queue);
                _repository.Save();
                _logger.LogInfo("New queue was successfully created");

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(Create)} action {ex}");
                return View();
            }
        }
    }
}
