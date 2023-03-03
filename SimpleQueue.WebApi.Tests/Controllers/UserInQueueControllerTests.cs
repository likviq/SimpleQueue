using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.WebApi.Controllers;
using SimpleQueue.WebApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleQueue.WebApi.Tests.Controllers
{
    public class UserInQueueControllerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUserInQueueService> _mockUserInQueueService;
        private readonly Mock<IQueueService> _mockQueueService;
        private readonly Mock<ILogger<UserInQueueController>> _mockLogger;
        private readonly UserInQueueController _controller;
        private readonly Fixture _fixture;

        public UserInQueueControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockUserInQueueService = new Mock<IUserInQueueService>();
            _mockQueueService = new Mock<IQueueService>();
            _mockLogger = new Mock<ILogger<UserInQueueController>>();

            _controller = new UserInQueueController(
                _mockMapper.Object,
                _mockUserInQueueService.Object,
                _mockQueueService.Object,
                _mockLogger.Object);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task DeleteParticipant_QueueIsNotNullAndUserIsOwner_NoContent()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();
            var participant = FakeDb.UserInQueues.Where(q => q.QueueId == queueId).First();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, queue.OwnerId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            _mockUserInQueueService.Setup(x => x.GetAsync(participant.Id))
                .ReturnsAsync(participant);

            _mockUserInQueueService.Setup(x => x.Delete(participant));

            //Act
            var result = await _controller.DeleteParticipant(queueId, participant.Id);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteParticipant_UserIsNotOwnerAndParticipant_Forbid()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();
            var participant = FakeDb.UserInQueues.Where(q => q.QueueId == queueId).First();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "d76bb0e2-a57b-47e3-b937-50ab501909de")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            _mockUserInQueueService.Setup(x => x.GetAsync(participant.Id))
                .ReturnsAsync(participant);

            //Act
            var result = await _controller.DeleteParticipant(queueId, participant.Id);

            //Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task DeleteParticipant_ParticipantIsNotInQueue_NotFound()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();
            var participant = FakeDb.UserInQueues.Where(q => q.QueueId != queueId).First();

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            _mockUserInQueueService.Setup(x => x.GetAsync(participant.Id))
                .ReturnsAsync(participant);

            //Act
            var result = await _controller.DeleteParticipant(queueId, participant.Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteParticipant_ParticipantIsNull_NotFound()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();
            var participantId = Guid.Empty;

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            _mockUserInQueueService.Setup(x => x.GetAsync(participantId))
                .ReturnsAsync(It.IsAny<UserInQueue>);

            //Act
            var result = await _controller.DeleteParticipant(queueId, participantId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteParticipant_QueueIsNull_NotFound()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var participantId = Guid.Empty;

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(It.IsAny<Queue>);

            //Act
            var result = await _controller.DeleteParticipant(queueId, participantId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EnterQueue_QueueIsNotNullAndUserIsNotInQueue_NoContent()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();
            var participant = FakeDb.UserInQueues.Where(q => q.QueueId == queueId).First();
            var userId = Guid.Parse("d76bb0e2-a57b-47e3-b937-50ab501909de");

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            _mockUserInQueueService.Setup(x => x.IsUserInQueueAsync(userId, queueId))
                .ReturnsAsync(false);

            _mockUserInQueueService.Setup(x => x.InitializeUserInQueueAsync(userId, queueId))
                .ReturnsAsync(participant);

            _mockMapper.Setup(x => x.Map<UserInQueueViewModel>(It.IsAny<UserInQueue>()))
                .Returns(It.IsAny<UserInQueueViewModel>());

            //Act
            var result = await _controller.EnterQueue(queueId);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task EnterQueue_QueueIsNotNullAndUserIsInQueue_UnprocessableEntity()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();
            var participant = FakeDb.UserInQueues.Where(q => q.QueueId == queueId).First();
            var userId = Guid.Parse("d76bb0e2-a57b-47e3-b937-50ab501909de");

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            _mockUserInQueueService.Setup(x => x.IsUserInQueueAsync(userId, queueId))
                .ReturnsAsync(true);

            //Act
            var result = await _controller.EnterQueue(queueId);

            //Assert
            Assert.IsType<UnprocessableEntityResult>(result);
        }

        [Fact]
        public async Task EnterQueue_QueueIsNull_NotFound()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(It.IsAny<Queue>);

            //Act
            var result = await _controller.EnterQueue(queueId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EnterDelayedQueue_QueueIsNotNullAndDestinationIsInQueue_Ok()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();
            var participant = FakeDb.UserInQueues.Where(q => q.QueueId == queueId).First();
            var userId = Guid.Parse("d76bb0e2-a57b-47e3-b937-50ab501909de");

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            _mockUserInQueueService.Setup(x => x.IsDestinationInQueueAsync(queueId, participant.Id))
                .ReturnsAsync(true);

            _mockUserInQueueService.Setup(x => x.SetUserWithDestinationAsync(userId, participant.Id))
                .ReturnsAsync(participant);

            _mockMapper.Setup(x => x.Map<UserInDelayedQueueViewModel>(participant))
                .Returns(It.IsAny<UserInDelayedQueueViewModel>());

            //Act
            var result = await _controller.EnterQueue(queueId, participant.Id);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task EnterDelayedQueue_QueueIsNotNullAndNoDestinationInQueue_UnprocessableEntity()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();
            var participant = FakeDb.UserInQueues.Where(q => q.QueueId == queueId).First();

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            _mockUserInQueueService.Setup(x => x.IsDestinationInQueueAsync(queueId, participant.Id))
                .ReturnsAsync(false);

            //Act
            var result = await _controller.EnterQueue(queueId, participant.Id);

            //Assert
            Assert.IsType<UnprocessableEntityResult>(result);
        }

        [Fact]
        public async Task EnterDelayedQueue_QueueIsNull_NotFound()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var participant = FakeDb.UserInQueues.Where(q => q.QueueId == queueId).First();

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(It.IsAny<Queue>);

            //Act
            var result = await _controller.EnterQueue(queueId, participant.Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ChangePosition_QueueIsNotNullUserInQueueNotOwnerAndParticipantsInSameQueue_Ok()
        {
            //Arrange
            var queueId = Guid.Parse("96f4cb8d-4c79-48a0-ae18-74fc1bc10225");
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();
            
            var participant = FakeDb.UserInQueues.Where(q => q.QueueId == queueId).First();
            var targetParticipant = FakeDb.UserInQueues.Where(q => q.QueueId == queueId).Last();
            
            var userId = FakeDb.Users.Where(user => user.Id != queue.OwnerId 
                && user.Id == participant.UserId).First().Id;

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            _mockUserInQueueService.Setup(x => x.IsUserInQueueAsync(userId, queueId))
                .ReturnsAsync(true);

            _mockUserInQueueService.Setup(x => x.GetAsync(participant.Id))
                .ReturnsAsync(participant);
            _mockUserInQueueService.Setup(x => x.GetAsync(targetParticipant.Id))
                .ReturnsAsync(targetParticipant);

            _mockUserInQueueService.Setup(x => x.MoveUserInQueueAfter(participant, targetParticipant));

            //Act
            var result = await _controller.ChangePosition(queueId, participant.Id, targetParticipant.Id);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ChangePosition_UserTriesToChangeThePlaceOutOfTurn_Forbid()
        {
            //Arrange
            var queueId = Guid.Parse("96f4cb8d-4c79-48a0-ae18-74fc1bc10225");
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();

            var participant = FakeDb.UserInQueues.Where(q => q.QueueId == queueId).First();
            var targetParticipant = FakeDb.UserInQueues.Where(q => q.QueueId == queueId).Last();

            var userId = FakeDb.Users.Where(user => user.Id != queue.OwnerId
                && user.Id != participant.UserId).First().Id;

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            _mockUserInQueueService.Setup(x => x.IsUserInQueueAsync(userId, queueId))
                .ReturnsAsync(true);

            _mockUserInQueueService.Setup(x => x.GetAsync(participant.Id))
                .ReturnsAsync(participant);
            _mockUserInQueueService.Setup(x => x.GetAsync(targetParticipant.Id))
                .ReturnsAsync(targetParticipant);

            //Act
            var result = await _controller.ChangePosition(queueId, participant.Id, targetParticipant.Id);

            //Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task ChangePosition_UsersInDifferentQueues_BadRequest()
        {
            //Arrange
            var queueId = Guid.Parse("96f4cb8d-4c79-48a0-ae18-74fc1bc10225");
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();

            var participant = FakeDb.UserInQueues.Where(q => q.QueueId == queueId).First();
            var targetParticipant = FakeDb.UserInQueues.Where(q => q.QueueId != queueId).First();

            var userId = FakeDb.Users.Where(user => user.Id != queue.OwnerId
                && user.Id != participant.UserId).First().Id;

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            _mockUserInQueueService.Setup(x => x.IsUserInQueueAsync(userId, queueId))
                .ReturnsAsync(true);

            _mockUserInQueueService.Setup(x => x.GetAsync(participant.Id))
                .ReturnsAsync(participant);
            _mockUserInQueueService.Setup(x => x.GetAsync(targetParticipant.Id))
                .ReturnsAsync(targetParticipant);

            //Act
            var result = await _controller.ChangePosition(queueId, participant.Id, targetParticipant.Id);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task ChangePosition_NoUserInQueue_NotFound()
        {
            //Arrange
            var queueId = Guid.Parse("96f4cb8d-4c79-48a0-ae18-74fc1bc10225");
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();

            var participant = FakeDb.UserInQueues.Where(q => q.QueueId == queueId).First();
            var targetParticipant = FakeDb.UserInQueues.Where(q => q.QueueId != queueId).First();

            var userId = FakeDb.Users.Where(user => user.Id != queue.OwnerId
                && user.Id != participant.UserId).First().Id;

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            _mockUserInQueueService.Setup(x => x.IsUserInQueueAsync(userId, queueId))
                .ReturnsAsync(false);

            //Act
            var result = await _controller.ChangePosition(queueId, participant.Id, targetParticipant.Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ChangePosition_QueueIsNull_NotFound()
        {
            //Arrange
            var queueId = Guid.Parse("96f4cb8d-4c79-48a0-ae18-74fc1bc10225");

            var participant = FakeDb.UserInQueues.Where(q => q.QueueId == queueId).First();
            var targetParticipant = FakeDb.UserInQueues.Where(q => q.QueueId != queueId).First();

            _mockQueueService.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<Queue>);

            //Act
            var result = await _controller.ChangePosition(queueId, participant.Id, targetParticipant.Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
