using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.WebUI.Controllers;
using SimpleQueue.WebUI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleQueue.WebUI.Tests.Controllers
{
    public class GetQueueControllerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IQueueService> _mockQueueService;
        private readonly Mock<IUserInQueueService> _mockUserInQueueService;
        private readonly Mock<ITagService> _mockTagService;

        private readonly Mock<IQueueTagService> _mockQueueTagService;
        private readonly Mock<IQueueTypeService> _mockQueueTypeService;
        private readonly Mock<IQrCodeGenerator> _mockQrCodeGenerator;
        private readonly Mock<IAzureStorage> _mockAzureStorage;

        private readonly Mock<ILogger<QueueController>> _mockLogger;
        private readonly QueueController _controller;
        private readonly Fixture _fixture;

        const string keyError = "key";
        const string messageError = "value";

        public GetQueueControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockQueueService = new Mock<IQueueService>();
            _mockUserInQueueService = new Mock<IUserInQueueService>();
            _mockTagService = new Mock<ITagService>();

            _mockQueueTagService = new Mock<IQueueTagService>();
            _mockQueueTypeService = new Mock<IQueueTypeService>();
            _mockQrCodeGenerator = new Mock<IQrCodeGenerator>();
            _mockAzureStorage = new Mock<IAzureStorage>();

            _mockLogger = new Mock<ILogger<QueueController>>();

            _controller = new QueueController(
                _mockMapper.Object,
                _mockQueueService.Object,
                _mockUserInQueueService.Object,
                _mockTagService.Object,

                _mockQueueTagService.Object,
                _mockQueueTypeService.Object,
                _mockQrCodeGenerator.Object,
                _mockAzureStorage.Object,
                _mockLogger.Object);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task Get_QueueUnauthorized_ReturnsQueue()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.First();

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            var queueViewModel = _fixture.Build<GetQueueViewModel>()
                .With(prop => prop.YourId, Guid.Empty)
                .Create();

            _mockMapper.Setup(x => x.Map<GetQueueViewModel>(queue))
                .Returns(queueViewModel);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };

            //Act
            var result = await _controller.GetAsync(queueId);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<GetQueueViewModel>(
                viewResult.ViewData.Model);
            Assert.NotNull(result);
            Assert.Equal(model.YourId, Guid.Empty);
        }

        [Fact]
        public async Task Get_QueueAuthorized_ReturnsQueue()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.First();
            var userId = "88d5fabc-0f6a-420d-84df-c8f26633a440";
            var participantPlace = 3;

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            var queueViewModel = _fixture.Build<GetQueueViewModel>()
                .With(prop => prop.YourId, Guid.Empty)
                .Create();

            _mockMapper.Setup(x => x.Map<GetQueueViewModel>(queue))
                .Returns(queueViewModel);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _mockUserInQueueService.Setup(x => x.UserPositionInQueueAsync(
                Guid.Parse(userId), queueId))
                .ReturnsAsync(participantPlace);

            //Act
            var result = await _controller.GetAsync(queueId);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<GetQueueViewModel>(
                viewResult.ViewData.Model);
            Assert.NotNull(result);
            Assert.Equal(model.YourId, Guid.Parse(userId));
        }

        [Fact]
        public async Task Get_QueueIdIsNull_ReturnsBadRequest()
        {
            //Arrange
            _mockMapper.Setup(x => x.Map<GetQueueViewModel>(It.IsAny<Queue>()))
                .Returns(It.IsAny<GetQueueViewModel>());

            _mockQueueService.Setup(option => option.GetAsync(It.IsAny<Guid>()));

            //Act
            var result = await _controller.GetAsync(It.IsAny<Guid>());

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
