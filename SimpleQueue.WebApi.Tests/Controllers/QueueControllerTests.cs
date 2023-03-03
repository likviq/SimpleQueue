using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Domain.RequestFeatures;
using SimpleQueue.WebApi.Controllers;
using SimpleQueue.WebApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace SimpleQueue.WebApi.Tests.Controllers
{
    public class QueueControllerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IQueueService> _mockQueueService;
        private readonly Mock<ILogger<QueueController>> _mockLogger;
        private readonly QueueController _controller;
        private readonly Fixture _fixture;

        public QueueControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockQueueService = new Mock<IQueueService>();
            _mockLogger = new Mock<ILogger<QueueController>>();

            _controller = new QueueController(
                _mockMapper.Object,
                _mockQueueService.Object,
                _mockLogger.Object);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task FreezeQueue_QueueIsNotNullAndUserIsOwner_Ok()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).FirstOrDefault();

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "9355a7df-c059-4c23-8c1f-31ec31962944")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _mockQueueService.Setup(x => x.FreezeAsync(queueId));
            //Act
            var result = await _controller.FreezeQueue(queueId);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task FreezeQueue_QueueIsNotNullAndUserIsNotOwner_Unauthorized()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).FirstOrDefault();

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "cccc3ce5-9a09-4b41-bc16-6c2e13007066")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //Act
            var result = await _controller.FreezeQueue(queueId);

            //Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task FreezeQueue_QueueIsNull_Unauthorized()
        {
            //Arrange
            var queueId = new Guid();

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(It.IsAny<Queue>);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "a131271b-4033-4985-903c-a5bc962f74be")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //Act
            var result = await _controller.FreezeQueue(queueId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task NextParticipant_QueueIsNotNullAndUserIsOwnerAndQueueHasParticipant_NoContent()
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

            _mockQueueService.Setup(x => x.NextParticipantAsync(queueId))
                .ReturnsAsync(participant);

            _mockQueueService.Setup(x => x.DeleteParticipant(participant));

            //Act
            var result = await _controller.NextParticipant(queueId);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task NextParticipant_QueueIsNotNullAndUserIsOwnerAndQueueHasNotParticipant_NoContent()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();

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

            _mockQueueService.Setup(x => x.NextParticipantAsync(queueId))
                .ReturnsAsync(It.IsAny<UserInQueue>());

            //Act
            var result = await _controller.NextParticipant(queueId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task NextParticipant_QueueIsNotNullAndUserIsNotOwner_Unauthorized()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();
            var userNotOwnerId = "cccc3ce5-9a09-4b41-bc16-6c2e13007066";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userNotOwnerId)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            //Act
            var result = await _controller.NextParticipant(queueId);

            //Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task NextParticipant_QueueIsNull_NotFound()
        {
            var queueId = _fixture.Create<Guid>();

            //Arrange
            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(It.IsAny<Queue>);

            //Act
            var result = await _controller.NextParticipant(queueId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteQueue_QueueIsNotNullAndUserIsOwner_NoContent()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();

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

            _mockQueueService.Setup(x => x.Delete(queue));

            //Act
            var result = await _controller.DeleteQueue(queueId);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteQueue_QueueIsNotNullAndUserIsNotOwner_Forbid()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.Where(q => q.Id == queueId).First();
            var userNotOwnerId = "cccc3ce5-9a09-4b41-bc16-6c2e13007066";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userNotOwnerId)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            //Act
            var result = await _controller.DeleteQueue(queueId);

            //Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task GetQueues_QueueParamsIsValid_QueueList()
        {
            //Arrange
            QueueParameters queueParams = _fixture.Build<QueueParameters>()
                .With(param => param.StartTime, new DateTime(2023, 3, 3))
                .With(param => param.EndTime, new DateTime(2023, 3, 4))
                .Create();

            List<Queue> queues = FakeDb.Queues;

            PagedList<Queue> pagedList = PagedList<Queue>.ToPagedList(
                queues, queueParams.PageNumber, queueParams.PageSize);

            _mockQueueService.Setup(x => x.GetQueuesAsync(queueParams, true))
                .ReturnsAsync(pagedList);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };

            _mockMapper.Setup(x => x.Map<List<QueueSearchResultViewModel>>(pagedList))
                .Returns(It.IsAny<List<QueueSearchResultViewModel>>());

            //Act
            var result = await _controller.GetQueues(queueParams);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetQueues_QueueParamsIsInValid_QueueList()
        {
            //Arrange
            QueueParameters queueParams = _fixture.Build<QueueParameters>()
                .With(param => param.StartTime, new DateTime(2023, 3, 4))
                .With(param => param.EndTime, new DateTime(2023, 3, 3))
                .Create();

            //Act
            var result = await _controller.GetQueues(queueParams);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
