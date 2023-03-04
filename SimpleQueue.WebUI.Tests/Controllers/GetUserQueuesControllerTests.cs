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
    public class GetUserQueuesControllerTests
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

        public GetUserQueuesControllerTests()
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
        public async Task GetUserQueues_UserIdIsNull_ReturnsBadRequest()
        {
            //Arrange
            var userId = Guid.Empty;
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //Act
            var result = await _controller.GetUserQueuesAsync();

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetUserQueues_UserIdNotNull_ReturnsUserQueues()
        {
            //Arrange
            var userId = "88d5fabc-0f6a-420d-84df-c8f26633a440";
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _mockQueueService.Setup(x => x.GetAllOwnerQueuesAsync(Guid.Parse(userId)))
                .ReturnsAsync(It.IsAny<List<Queue>>);

            _mockQueueService.Setup(x => x.GetAllParticipantQueuesAsync(Guid.Parse(userId)))
                .ReturnsAsync(It.IsAny<List<Queue>>);

            _mockMapper.Setup(x => x.Map<List<BriefQueueInfoViewModel>>(It.IsAny<List<Queue>>()))
                .Returns(It.IsAny<List<BriefQueueInfoViewModel>>());

            //Act
            var result = await _controller.GetUserQueuesAsync();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AllUserQueuesViewModel>(
                viewResult.ViewData.Model);
            Assert.NotNull(result);
        }
    }
}
