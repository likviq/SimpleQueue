using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;
using System.Security.Claims;

namespace SimpleQueue.WebApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserInQueueController : ControllerBase
    {
        private readonly IUserInQueueService _userInQueueService;
        private readonly IQueueService _queueService;
        private readonly ILogger<UserInQueueController> _logger;

        public UserInQueueController(IUserInQueueService userInQueueService, IQueueService queueService, ILogger<UserInQueueController> logger)
        {
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

                var queue = await _queueService.GetQueue(queueId);
                if (queue == null)
                {
                    _logger.LogWarning($"Queue with id - {queueId} not found");
                    return NotFound();
                }

                var userInQueue = await _userInQueueService.GetUserInQueue(userInQueueId);
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

                var queue = await _queueService.GetQueue(queueId);
                if (queue == null)
                {
                    _logger.LogWarning($"Queue with id - {queueId} not found");
                    return NotFound();
                }

                var isUserInQueue = _userInQueueService.IsUserInQueue(userId, queueId);
                if (isUserInQueue)
                {
                    _logger.LogWarning($"User with id - {userId} already in queue with id - {queueId}");
                    return UnprocessableEntity();
                }

                var userInQueue = await _userInQueueService.InitializeUserInQueue(userId, queueId);
                _logger.LogInformation($"User with id - {userId} has been added to the queue with id - {queueId}." +
                    $"Id in {nameof(UserInQueue)} object is {userInQueue.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method {nameof(EnterQueue)} from the controller {nameof(UserInQueueController)} " +
                    $"was broken due to an error: {ex.Message}");
            }
            _logger.LogInformation($"Method {nameof(EnterQueue)} from the controller {nameof(UserInQueueController)} " +
                $"completed successfully");

            return Ok();
        }
    }
}
