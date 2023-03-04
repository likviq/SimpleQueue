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
        private readonly ITagService _tagService;
        private readonly IQueueTagService _queueTagService;
        private readonly IQueueTypeService _queueTypeService;
        private readonly IQrCodeGenerator _qrCodeGenerator;
        private readonly IAzureStorage _azureStorage;
        private readonly ILogger<QueueController> _logger;
        public QueueController(
            IMapper mapper, 
            IQueueService queueService, 
            IUserInQueueService userInQueueService,
            ITagService tagService,
            IQueueTagService queueTagService,
            IQueueTypeService queueTypeService,
            IQrCodeGenerator qrCodeGenerator,
            IAzureStorage azureStorage,
            ILogger<QueueController> logger)
        {
            _mapper = mapper;
            _queueService = queueService;
            _userInQueueService = userInQueueService;
            _tagService = tagService;
            _queueTagService = queueTagService;
            _queueTypeService = queueTypeService;
            _qrCodeGenerator = qrCodeGenerator;
            _azureStorage = azureStorage;
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

            var queue = await _queueService.GetAsync(id);
            
            if (queue == null)
            {
                _logger.LogError($"Queue with id - {id} does not exist");
                return NotFound();
            }

            _logger.LogInformation($"Queue with id - {id} has been received");

            var queueViewModel = _mapper.Map<GetQueueViewModel>(queue);
            _logger.LogInformation($"Queue with id - {id} has been converted to an object {nameof(GetQueueViewModel)}");

            if (User.Identity.IsAuthenticated != false)
            {
                var identityUserId = new Guid(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

                queueViewModel.YourId = identityUserId;
                _logger.LogInformation($"User with id - {identityUserId} visit queue with id - {id}");

                queueViewModel.YourPosition = await _userInQueueService.UserPositionInQueueAsync(identityUserId, queueViewModel.Id);
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

            var ownerQueues = await _queueService.GetAllOwnerQueuesAsync(userId);
            if (ownerQueues == null)
            {
                _logger.LogWarning($"No queue was found that is owned by user with id - {userId}");
            }

            var participantQueues = await _queueService.GetAllParticipantQueuesAsync(userId);
            if (participantQueues == null)
            {
                _logger.LogWarning($"No queues were found with user id - {userId} as a member");
            }

            var ownerQueuesViewModel = _mapper.Map<List<BriefQueueInfoViewModel>>(ownerQueues);
            _logger.LogInformation($"Owner queues with has been converted to the list of object {nameof(BriefQueueInfoViewModel)}");

            var participantQueuesViewModel = _mapper.Map<List<BriefQueueInfoViewModel>>(participantQueues);
            _logger.LogInformation($"Participant queues with has been converted to the list of object {nameof(BriefQueueInfoViewModel)}");

            var queues = new AllUserQueuesViewModel(ownerQueuesViewModel, participantQueuesViewModel);
            _logger.LogInformation($"Two models were successfully converted to {nameof(AllUserQueuesViewModel)}");

            return View(queues);
        }

        [Authorize]
        [HttpGet("/queue")]
        public IActionResult Create()
        {
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
            _logger.LogInformation($"{nameof(CreateQueueDto)} object has been converted to the {nameof(Queue)} entity");

            var queueType = await _queueTypeService.GetQueueTypeAsync(TypeName.Fast);

            if (createQueueDto.IsDelayed)
            {
                var fromTime = (DateTime)createQueueDto.DelayedTimeFrom;
                var toTime = (DateTime)createQueueDto.DelayedTimeTo;
                var duration = (int)createQueueDto.DurationPerParticipant;

                var delayedPlaces = _userInQueueService.CreateDelayedPlaces(
                    fromTime, toTime, duration);
                _logger.LogInformation($"Successfully created places for the delayed queue" +
                    $"with title - {queue.Title}");

                queue.UserInQueues = delayedPlaces;

                queueType = await _queueTypeService.GetQueueTypeAsync(TypeName.Delayed);
            }

            queue.QueueType = queueType;

            var tagsDto = createQueueDto.TagsDto;
            if (tagsDto.Any())
            {
                var tags = _mapper.Map<List<Tag>>(tagsDto);
                await _tagService.CreateManyAsync(tags);

                var queueTags = await _queueTagService.InitializeTagsAsync(tags);
                _logger.LogInformation($"{queueTags.Count} tags have been created " +
                    $"for the queue with the name - {queue.Title}");

                queue.QueueTags = queueTags;
            }

            if (createQueueDto.ImageFile != null)
            {
                var imageFile = createQueueDto.ImageFile;

                var imageBlob = await _azureStorage.UploadAsync(imageFile);
                _logger.LogInformation($"Image with name - {imageBlob.Name} successfully uploaded to the storage");

                queue.ImageBlob = imageBlob;
            }

            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            queue.OwnerId = new Guid(userId);

            await _queueService.CreateAsync(queue);
            _logger.LogInformation("New queue was successfully created");

            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpGet("/queues")]
        public async Task<IActionResult> GetQueuesAsync([FromQuery] QueueParameters queueParameters)
        {
            var client = new HttpClient();

            var isFrozenQuery = queueParameters.IsFrozen == null ? "" : $"&IsFrozen={queueParameters.IsFrozen}";
            var isChatQuery = queueParameters.IsChat == null ? "" : $"&IsChat={queueParameters.IsChat}";
            var isProtectedQuery = queueParameters.IsProtected == null ? "" : $"&IsProtected={queueParameters.IsProtected}";
            var sortByQuery = queueParameters.SortBy == null ? "" : $"&SortBy={queueParameters.SortBy}";

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = new HttpMethod("get"),
                RequestUri = new Uri($"https://localhost:7147/api/queues" +
                $"?SearchTerm={queueParameters.SearchTerm}" +
                $"&StartTime={queueParameters.StartTime}" +
                $"&EndTime={queueParameters.EndTime}" +
                isFrozenQuery + isChatQuery + isProtectedQuery + sortByQuery)
            };

            _logger.LogInformation("Request for searching queues have been prepared");

            HttpResponseMessage response = await client.SendAsync(request);

            var responseContent = await response.Content.ReadFromJsonAsync<List<QueueSearchResultViewModel>>();
            _logger.LogInformation($"Were found {responseContent.Count} queues");

            return View(responseContent);
        }

        [HttpGet("/queue/{id}/qrcode")]
        public async Task<IActionResult> QrCodeAsync(Guid id)
        {
            var queue = await _queueService.GetAsync(id);

            if (queue == null)
            {
                _logger.LogError($"Queue with id - {id} does not exist");
                return NotFound();
            }
            _logger.LogInformation($"Queue with id - {id} for qr code generate has been received");

            var queueViewModel = _mapper.Map<QrCodeViewModel>(queue);
            _logger.LogInformation($"Queue with id - {id} has been converted " +
                $"to an object {nameof(QrCodeViewModel)}");


            string baseUrl = string.Format("{0}://{1}",
                       HttpContext.Request.Scheme, HttpContext.Request.Host);
            string complexUrl = baseUrl + "/queue/" + id;
            _logger.LogInformation($"Link to the queue for generating the qr code has been created");

            var svgImage = await _qrCodeGenerator.GenerateQrCodeAsync(complexUrl);
            _logger.LogInformation($"Svg image has been created");

            queueViewModel.SvgImage = svgImage;
            queueViewModel.QueueLink = complexUrl;

            return View(queueViewModel);
        }
    }
}
