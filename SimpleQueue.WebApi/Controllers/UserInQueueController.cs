using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.WebApi.Models.ViewModels;
using System.Security.Claims;

namespace SimpleQueue.WebApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserInQueueController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserInQueueService _userInQueueService;
        private readonly IQueueService _queueService;
        private readonly ILogger<UserInQueueController> _logger;

        public UserInQueueController(
            IMapper mapper, 
            IUserInQueueService userInQueueService, 
            IQueueService queueService, 
            ILogger<UserInQueueController> logger)
        {
            _mapper = mapper;
            _userInQueueService = userInQueueService;
            _queueService = queueService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("queue/{queueId}/participant/{userInQueueId}")]
        public async Task<IActionResult> DeleteUser(Guid queueId, Guid userInQueueId)
        {
            try
            {
                var userId = new Guid(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

                var queue = await _queueService.GetAsync(queueId);
                if (queue == null)
                {
                    _logger.LogWarning($"Queue with id - {queueId} not found");
                    return NotFound();
                }

                var userInQueue = await _userInQueueService.GetAsync(userInQueueId);
                if (userInQueue == null)
                {
                    _logger.LogWarning($"UserInQueue with id - {userInQueueId} not found");
                    return NotFound();
                }

                if (userInQueue.QueueId != queueId)
                {
                    _logger.LogWarning($"Queue with id - {queueId} has not participant with id - {userInQueueId}");
                    return NotFound();
                }

                if (queue.OwnerId != userId && userId != userInQueue.UserId)
                {
                    _logger.LogWarning($"Someone with id - {userId} tried to delete a UserInQueue " +
                        $"with id - {userInQueueId} from queue with id - {queueId}");
                    return Forbid();
                }

                _userInQueueService.Delete(userInQueue);
                _logger.LogInformation($"{nameof(UserInQueue)} object with id - {userInQueueId} successfully deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method {nameof(DeleteUser)} from the controller {nameof(UserInQueueController)} " +
                    $"was broken due to an error: {ex.Message}");
                return BadRequest();
            }
            _logger.LogInformation($"Method {nameof(DeleteUser)} from the controller {nameof(UserInQueueController)} " +
                $"completed successfully");

            return NoContent();
        }

        [Authorize]
        [HttpPost("queue/{queueId}/enter")]
        public async Task<IActionResult> EnterQueue(Guid queueId)
        {
            try
            {
                var userId = new Guid(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

                var queue = await _queueService.GetAsync(queueId);
                if (queue == null)
                {
                    _logger.LogWarning($"Queue with id - {queueId} not found");
                    return NotFound();
                }

                var isUserInQueue = await _userInQueueService.IsUserInQueueAsync(userId, queueId);
                if (isUserInQueue)
                {
                    _logger.LogWarning($"User with id - {userId} already in queue with id - {queueId}");
                    return UnprocessableEntity();
                }

                var userInQueue = await _userInQueueService.InitializeUserInQueueAsync(userId, queueId);
                _logger.LogInformation($"User with id - {userId} has been added to the queue with id - {queueId}." +
                    $"Id in {nameof(UserInQueue)} object is {userInQueue.Id}");

                var userInQueueViewModel = _mapper.Map<UserInQueueViewModel>(userInQueue);
                
                _logger.LogInformation($"{nameof(UserInQueueViewModel)} object successfully " +
                    $"created from {nameof(UserInQueue)}");

                _logger.LogInformation($"Method {nameof(EnterQueue)} from the controller {nameof(UserInQueueController)} " +
                $"completed successfully");

                return Ok(userInQueueViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method {nameof(EnterQueue)} from the controller {nameof(UserInQueueController)} " +
                    $"was broken due to an error: {ex.Message}");
                return BadRequest();
            }           
        }

        [Authorize]
        [HttpPost("queue/{queueId}/participant/{userInQueueId}/delayed")]
        public async Task<IActionResult> EnterQueue(Guid queueId, Guid userInQueueId)
        {
            try
            {
                var userId = new Guid(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

                var queue = await _queueService.GetAsync(queueId);
                if (queue == null)
                {
                    _logger.LogWarning($"Queue with id - {queueId} not found");
                    return NotFound();
                }

                var isUserInQueue = await _userInQueueService.IsDestinationInQueueAsync(queueId, userInQueueId);
                if (!isUserInQueue)
                {
                    _logger.LogWarning($"Place with participant id - {userInQueueId} " +
                        $"in queue with id - {queueId} is already taken or doesn't exist");
                    return UnprocessableEntity();
                }

                var userWithDestination = await _userInQueueService.SetUserWithDestinationAsync(
                    userId, userInQueueId);
                _logger.LogInformation($"User with id - {userId} has been added to the queue with " +
                    $"id - {queueId}");

                var userInDelayedQueueViewModel = _mapper
                    .Map<UserInDelayedQueueViewModel>(userWithDestination);
                _logger.LogInformation($"{nameof(UserInDelayedQueueViewModel)} object successfully " +
                    $"created from {nameof(UserInQueue)}");

                _logger.LogInformation($"Method {nameof(EnterQueue)} from the controller " +
                    $"{nameof(UserInQueueController)} completed successfully");

                return Ok(userInDelayedQueueViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method {nameof(EnterQueue)} from the controller " +
                    $"{nameof(UserInQueueController)} was broken due to an error: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("queue/{queueId}/participant/{userInQueueId}/after/{targetUserInQueueId}")]
        public async Task<IActionResult> ChangePosition(Guid queueId, Guid userInQueueId, Guid targetUserInQueueId)
        {
            var userId = new Guid(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var queue = await _queueService.GetAsync(queueId);
            if (queue == null)
            {
                _logger.LogWarning($"Queue with id - {queueId} not found");
                return NotFound();
            }

            var isUserInQueue = await _userInQueueService.IsUserInQueueAsync(userId, queueId);
            if (!isUserInQueue && queue.OwnerId != userId)
            {
                _logger.LogWarning($"There is no user with id - {userId} in queue with id - {queueId}");
                return NotFound();
            }

            var userInQueue = await _userInQueueService.GetAsync(userInQueueId);
            var targetUserInQueue = await _userInQueueService.GetAsync(targetUserInQueueId);
            if (userInQueue.QueueId != queueId || targetUserInQueue.QueueId != userInQueue.QueueId)
            {
                _logger.LogWarning($"{userInQueueId} and {targetUserInQueueId} are in different queues");
                return BadRequest();
            }

            if (userInQueue.UserId != userId && queue.OwnerId != userId)
            {
                _logger.LogWarning($"Someone with id - {userId} tried to move a {nameof(UserInQueue)} " +
                        $"with id - {userInQueueId} from queue with id - {queueId}");
                return Forbid();
            }
            _userInQueueService.MoveUserInQueueAfter(userInQueue, targetUserInQueue);

            return Ok();
        }
    }
}
