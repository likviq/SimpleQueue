﻿using Microsoft.AspNetCore.Authorization;
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
        [HttpPost("queue/{queueId}")]
        public async Task<IActionResult> FreezeQueue(Guid queueId)
        {          
            try
            {
                var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                var queue = await _queueService.GetQueue(queueId);
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

                await _queueService.FreezeQueue(queueId);
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
                var userId = new Guid(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

                var queue = await _queueService.GetQueue(queueId);
                if (queue == null)
                {
                    _logger.LogWarning($"queue with id - {queueId} not found");
                    return NotFound();
                }

                if (queue.OwnerId != userId)
                {
                    _logger.LogWarning($"queue owner id is - {queue.OwnerId}, " +
                        $"but id from claims is - {userId}");
                    return Unauthorized();
                }

                var participant = await _queueService.NextParticipant(queueId);
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
                var userId = new Guid(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

                var queue = await _queueService.GetQueue(queueId);
                if (queue == null)
                {
                    _logger.LogWarning($"queue with id - {queueId} not found");
                    return NotFound();
                }

                if (queue.OwnerId != userId)
                {
                    _logger.LogWarning($"Someone with id - {userId} tried to delete a queue " +
                            $"with id - {queueId}");
                    return Forbid();
                }

                _queueService.DeleteQueue(queue);
                
                _logger.LogInformation($"User with id - {userId} successfully deleted own queue with id - {queueId}");
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to delete the queue due to an unexpected error - {ex.Message}");
                return BadRequest();
            }   
        }
    }
}