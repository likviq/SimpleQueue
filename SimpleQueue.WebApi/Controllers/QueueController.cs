using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleQueue.Domain.Interfaces;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpleQueue.WebApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly IQueueService _queueService;
        private readonly ILogger<QueueController> _logger;
        
        public QueueController(IQueueService queueService, ILogger<QueueController> logger)
        {
            _queueService = queueService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("queue/{id}")]
        public async Task<IActionResult> FreezeQueue(Guid id)
        {          
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                var queue = await _queueService.GetQueue(id);
                if (queue == null)
                {
                    _logger.LogWarning($"queue with id - {id} not found");
                    return NotFound();
                }

                if (queue.OwnerId != new Guid(userId))
                {
                    _logger.LogWarning($"queue owner id is - {queue.OwnerId}, " +
                        $"but id from claims is - {userId}");
                    return Unauthorized();
                }

                await _queueService.FreezeQueue(id);
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
        [HttpPost("queue/{id}/next")]
        public async Task<IActionResult> NextParticipant(Guid id)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                var queue = await _queueService.GetQueue(id);
                if (queue == null)
                {
                    _logger.LogWarning($"queue with id - {id} not found");
                    return NotFound();
                }

                if (queue.OwnerId != new Guid(userId))
                {
                    _logger.LogWarning($"queue owner id is - {queue.OwnerId}, " +
                        $"but id from claims is - {userId}");
                    return Unauthorized();
                }

                var participant = await _queueService.NextParticipant(id);
                if (participant == null)
                {
                    _logger.LogWarning($"queue with id - {id} has no members");
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
    }
}
