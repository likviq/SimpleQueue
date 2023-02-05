using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleQueue.Data;
using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.WebUI.Models.DataTransferObjects;
using SimpleQueue.WebUI.Models.ViewModels;

namespace SimpleQueue.WebUI.Controllers
{
    public class QueueController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IQueueService _queueService;
        private readonly ILoggerManager _logger;
        public QueueController(IMapper mapper, IQueueService queueService, ILoggerManager logger)
        {
            _mapper = mapper;
            _queueService = queueService;
            _logger = logger;
        }

        public async Task<IActionResult> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("id from the query is incorrect or equal to zero");
                return BadRequest();
            }

            var queue = await _queueService.GetQueue(id);
            
            if (queue == null)
            {
                _logger.LogError($"Queue with id - {id} does not exist");
                return NotFound();
            }

            _logger.LogInfo($"Queue with id - {id} has been received");

            var queueViewModel = _mapper.Map<GetQueueViewModel>(queue);
            _logger.LogInfo($"Queue with id - {id} has been converted to an object {nameof(GetQueueViewModel)}");

            return View(queueViewModel);
        }

        [HttpGet("/user/{userId}/queues")]
        public async Task<IActionResult> GetUserQueuesAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                _logger.LogError("id from the query is incorrect or equal to zero");
                return BadRequest();
            }

            var ownerQueues = await _queueService.GetAllOwnerQueues(userId);
            if (ownerQueues == null)
            {
                _logger.LogWarn($"No queue was found that is owned by user with id - {userId}");
            }

            var participantQueues = await _queueService.GetAllParticipantQueues(userId);
            if (participantQueues == null)
            {
                _logger.LogWarn($"No queues were found with user id - {userId} as a member");
            }

            var ownerQueuesViewModel = _mapper.Map<List<BriefQueueInfoViewModel>>(ownerQueues);
            _logger.LogInfo($"Owner queues with has been converted to the list of object {nameof(BriefQueueInfoViewModel)}");

            var participantQueuesViewModel = _mapper.Map<List<BriefQueueInfoViewModel>>(participantQueues);
            _logger.LogInfo($"Participant queues with has been converted to the list of object {nameof(BriefQueueInfoViewModel)}");

            var queues = new AllUserQueuesViewModel(ownerQueuesViewModel, participantQueuesViewModel);
            _logger.LogInfo($"Two models were successfully converted to {nameof(AllUserQueuesViewModel)}");

            return View(queues);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(CreateQueueDto? createQueueDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("createQueueDto object is null");
                return View();
            }

            var queue = _mapper.Map<Queue>(createQueueDto);

            await _queueService.CreateQueue(queue);
            _logger.LogInfo("New queue was successfully created");

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
