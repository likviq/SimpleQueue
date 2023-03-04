using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Domain.RequestFeatures;
using SimpleQueue.WebApi.Models.ViewModels;
using System.Security.Claims;

namespace SimpleQueue.WebApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQueueService _queueService;
        private readonly ILogger<QueueController> _logger;
        
        public QueueController(IMapper mapper, IQueueService queueService, ILogger<QueueController> logger)
        {
            _mapper = mapper;
            _queueService = queueService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("queue/{queueId}")]
        public async Task<IActionResult> FreezeQueue(Guid queueId)
        {          
            try
            {
                var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                var queue = await _queueService.GetAsync(queueId);
                if (queue == null)
                {
                    _logger.LogWarning($"queue with id - {queueId} not found");
                    return NotFound();
                }

                if (queue.OwnerId != new Guid(userId))
                {
                    _logger.LogWarning($"queue owner id is - {queue.OwnerId}, " +
                        $"but id from claims is - {userId}");
                    return Unauthorized();
                }

                await _queueService.FreezeAsync(queueId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to update the queue due to an unexpected error - {ex.Message}");
                return BadRequest();
            }

            _logger.LogInformation($"Update the queue freeze status");
            return Ok();
        }

        [Authorize]
        [HttpPost("queue/{queueId}/next")]
        public async Task<IActionResult> NextParticipant(Guid queueId)
        {
            try
            {
                var queue = await _queueService.GetAsync(queueId);
                if (queue == null)
                {
                    _logger.LogWarning($"queue with id - {queueId} not found");
                    return NotFound();
                }

                var userId = new Guid(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

                if (queue.OwnerId != userId)
                {
                    _logger.LogWarning($"queue owner id is - {queue.OwnerId}, " +
                        $"but id from claims is - {userId}");
                    return Unauthorized();
                }

                var participant = await _queueService.NextParticipantAsync(queueId);
                if (participant == null)
                {
                    _logger.LogWarning($"queue with id - {queueId} has no members");
                    return NotFound();
                }

                _queueService.DeleteParticipant(participant);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Unable to update the queue due to an unexpected error - {ex.Message}");
                return BadRequest();
            }
            _logger.LogInformation("Successfully changed the next participant");
            return NoContent();
        }

        [Authorize]
        [HttpDelete("queue/{queueId}")]
        public async Task<IActionResult> DeleteQueue(Guid queueId)
        {
            try
            {
                var queue = await _queueService.GetAsync(queueId);
                if (queue == null)
                {
                    _logger.LogWarning($"queue with id - {queueId} not found");
                    return NotFound();
                }

                var userId = new Guid(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

                if (queue.OwnerId != userId)
                {
                    _logger.LogWarning($"Someone with id - {userId} tried to delete a queue " +
                            $"with id - {queueId}");
                    return Forbid();
                }

                _queueService.Delete(queue);
                
                _logger.LogInformation($"User with id - {userId} successfully deleted own queue with id - {queueId}");
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to delete the queue due to an unexpected error - {ex.Message}");
                return BadRequest();
            }   
        }

        [HttpGet("queues")]
        public async Task<IActionResult> GetQueues([FromQuery] QueueParameters queueParameters)
        {
            if (!queueParameters.ValidTimeRange)
            {
                _logger.LogError($"End time of queue {queueParameters.EndTime} " +
                    $"is less than start time {queueParameters.StartTime}");
                return BadRequest();
            }

            var queues = await _queueService.GetQueuesAsync(queueParameters);
            if (queues == null)
            {
                _logger.LogWarning($"There are no queues with {nameof(queueParameters)} parameters" +
                    $"- {queueParameters}");
            }

            Response.Headers.Add("pagination",
                JsonConvert.SerializeObject(queues.MetaData));

            var queuesViewModel = _mapper.Map<List<QueueSearchResultViewModel>>(queues);
            _logger.LogInformation($"Founded queues have been converted to the list of object " +
                $"{nameof(QueueSearchResultViewModel)}");

            return Ok(queuesViewModel);
        }
    }
}
