using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimpleQueue.Data;
using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Domain.RequestFeatures;
using SimpleQueue.WebUI.Models.DataTransferObjects;
using SimpleQueue.WebUI.Models.ViewModels;
using System.Globalization;
using System.Security.Claims;

namespace SimpleQueue.WebUI.Controllers
{
    public class QueueController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IQueueService _queueService;
        private readonly IUserInQueueService _userInQueueService;
        private readonly ILoggerManager _logger;
        public QueueController(
            IMapper mapper, 
            IQueueService queueService, 
            IUserInQueueService userInQueueService, 
            ILoggerManager logger)
        {
            _mapper = mapper;
            _queueService = queueService;
            _userInQueueService = userInQueueService;
            _logger = logger;
        }

        [HttpGet("/queue/{id}")]
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

            if (User.Identity.IsAuthenticated != false)
            {
                var identityUserId = new Guid(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

                queueViewModel.YourId = identityUserId;
                _logger.LogInfo($"User with id - {identityUserId} visit queue with id - {id}");

                queueViewModel.YourPosition = await _userInQueueService.UserPositionInQueue(identityUserId, queueViewModel.Id);
            }

            return View(queueViewModel);
        }

        [Authorize]
        [HttpGet("/user/queues")]
        public async Task<IActionResult> GetUserQueuesAsync()
        {
            var userId = new Guid(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

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

        [Authorize]
        [HttpGet("/queue")]
        public IActionResult Create()
        {
            var token = HttpContext.GetTokenAsync("access_token");
            return View();
        }

        [Authorize]
        [HttpPost("/queue")]
        public async Task<IActionResult> CreateAsync(CreateQueueDto? createQueueDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("createQueueDto object is null");
                return View();
            }

            var queue = _mapper.Map<Queue>(createQueueDto);
            _logger.LogInfo($"{nameof(CreateQueueDto)} object has been converted to the {nameof(Queue)} entity");

            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            queue.OwnerId = new Guid(userId);

            await _queueService.CreateQueue(queue);
            _logger.LogInfo("New queue was successfully created");

            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpGet("/queues")]
        public async Task<IActionResult> GetQueues([FromQuery] QueueParameters queueParameters)
        {
            var client = new HttpClient();

            //DateTime date = DateTime.ParseExact(queueParameters.EndTime.ToString(), "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //string formattedDate = date.ToString("MM.dd.yyyy HH:mm:ss");  

            var isFrozenQuery = queueParameters.IsFrozen == null ? "" : $"&IsFrozen={queueParameters.IsFrozen}";
            var isChatQuery = queueParameters.IsChat == null ? "" : $"&IsChat={queueParameters.IsChat}";
            var isProtectedQuery = queueParameters.IsProtected == null ? "" : $"&IsProtected={queueParameters.IsProtected}";

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = new HttpMethod("get"),
                RequestUri = new Uri($"https://localhost:7147/api/queues" +
                $"?SearchTerm={queueParameters.SearchTerm}" +
                $"&StartTime={queueParameters.StartTime}" +
                $"&EndTime={queueParameters.EndTime}" +
                isFrozenQuery + isChatQuery + isProtectedQuery)
            };

            HttpResponseMessage response = await client.SendAsync(request);

            var responseContent = await response.Content.ReadFromJsonAsync<List<QueueSearchResultViewModel>>();
            return View(responseContent);
        }
    }
}
