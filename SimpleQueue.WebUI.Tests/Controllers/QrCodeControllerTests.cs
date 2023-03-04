using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.WebUI.Controllers;
using SimpleQueue.WebUI.Models.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleQueue.WebUI.Tests.Controllers
{
    public class QrCodeControllerTests
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
        private readonly HttpContext _httpContext;
        private readonly QueueController _controller;
        private readonly Fixture _fixture; 

        const string keyError = "key";
        const string messageError = "value";

        public QrCodeControllerTests()
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
            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Scheme = "https";
            _httpContext.Request.Host = HostString.FromUriComponent("host");

            _controller = new QueueController(
                _mockMapper.Object,
                _mockQueueService.Object,
                _mockUserInQueueService.Object,
                _mockTagService.Object,

                _mockQueueTagService.Object,
                _mockQueueTypeService.Object,
                _mockQrCodeGenerator.Object,
                _mockAzureStorage.Object,
                _mockLogger.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = _httpContext
                }
            };

            _fixture = new Fixture();
        }

        [Fact]
        public async Task QrCodeAsync_QueueExists_ReturnsQrCodeViewModel()
        {
            //Arrange
            var queueId = FakeDb.Queues.First().Id;
            var queue = FakeDb.Queues.Where(queue => queue.Id.Equals(queueId)).FirstOrDefault();
            var url = "JoinQueueLink";

            var qrcodeViewModel = _fixture.Build<QrCodeViewModel>().Create();

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            _mockMapper.Setup(x => x.Map<QrCodeViewModel>(queue))
                .Returns(qrcodeViewModel);

            _mockQrCodeGenerator.Setup(x => x.GenerateQrCodeAsync(url))
                .ReturnsAsync(It.IsAny<string>);

            //Act
            var result = await _controller.QrCodeAsync(queueId);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<QrCodeViewModel>(
                viewResult.ViewData.Model);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task QrCodeAsync_QueueDoesNotExists_ReturnsNotFound()
        {
            //Arrange
            var queueId = new Guid();
            var queue = FakeDb.Queues.Where(queue => queue.Id.Equals(queueId)).FirstOrDefault();

            _mockQueueService.Setup(x => x.GetAsync(queueId))
                .ReturnsAsync(queue);

            //Act
            var result = await _controller.QrCodeAsync(queueId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
