using Xunit;
using Moq;
using AutoMapper;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.WebUI.Controllers;
using Microsoft.AspNetCore.Mvc;
using AutoFixture;
using SimpleQueue.WebUI.Models.DataTransferObjects;
using SimpleQueue.Domain.Entities;
using SimpleQueue.WebUI.Automapper;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using SimpleQueue.WebUI.Models.ViewModels;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;

namespace SimpleQueue.WebUI.Tests.Controllers
{
    public class QueueControllerTests
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

        public QueueControllerTests()
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

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "88d5fabc-0f6a-420d-84df-c8f26633a440")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _fixture = new Fixture();
        }

        [Fact]
        public void Create_ActionExecutes_ReturnsViewForCreate()
        {
            var result = _controller.Create();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_FastQueueWithoutTagsAndImage_RedirectToActionHome()
        {
            //Arrange
            var queueDto = _fixture.Build<CreateQueueDto>()
                .Without(prop => prop.ImageFile)
                .Without(prop => prop.TagsDto)
                .With(prop => prop.IsDelayed, false)
                .Create();

            _mockMapper.Setup(x => x.Map<Queue>(It.IsAny<CreateQueueDto>()))
                .Returns(FakeDb.Queues.First());

            _mockQueueTypeService.Setup(x => x.GetQueueTypeAsync(It.IsAny<TypeName>()))
                .ReturnsAsync(FakeDb.QueueTypes.Where(type => type.Name.Equals(TypeName.Fast)).First());

            _mockQueueService.Setup(option => option.CreateAsync(It.IsAny<Queue>()));
            
            //Act
            var result = await _controller.CreateAsync(queueDto);

            //Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task Create_DelayedQueueWithoutTagsAndImage_RedirectToActionHome()
        {
            //Arrange
            var queueDto = _fixture.Build<CreateQueueDto>()
                .Without(prop => prop.ImageFile)
                .Without(prop => prop.TagsDto)
                .With(prop => prop.IsDelayed, true)
                .Create();

            _mockMapper.Setup(x => x.Map<Queue>(It.IsAny<CreateQueueDto>()))
                .Returns(FakeDb.Queues.First());

            _mockQueueTypeService.Setup(x => x.GetQueueTypeAsync(It.IsAny<TypeName>()))
                .ReturnsAsync(FakeDb.QueueTypes.Where(type => type.Name.Equals(TypeName.Delayed)).First());

            _mockUserInQueueService.Setup(x => x.CreateDelayedPlaces(
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(FakeDb.UserInQueuesDelayed);

            _mockQueueService.Setup(option => option.CreateAsync(It.IsAny<Queue>()));

            //Act
            var result = await _controller.CreateAsync(queueDto);

            //Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task Create_DelayedQueueWithTagsAndWithoutImage_RedirectToActionHome()
        {
            //Arrange
            var queueDto = _fixture.Build<CreateQueueDto>()
                .Without(prop => prop.ImageFile)
                .With(prop => prop.IsDelayed, true)
                .Create();

            _mockMapper.Setup(x => x.Map<Queue>(It.IsAny<CreateQueueDto>()))
                .Returns(FakeDb.Queues.First());

            _mockQueueTypeService.Setup(x => x.GetQueueTypeAsync(It.IsAny<TypeName>()))
                .ReturnsAsync(FakeDb.QueueTypes.Where(type => type.Name.Equals(TypeName.Delayed)).First());

            _mockQueueService.Setup(option => option.CreateAsync(It.IsAny<Queue>()));

            _mockUserInQueueService.Setup(x => x.CreateDelayedPlaces(
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(FakeDb.UserInQueuesDelayed);

            _mockMapper.Setup(x => x.Map<List<Tag>>(It.IsAny<IList<string>>()))
                .Returns(FakeDb.Tags);

            _mockTagService.Setup(x => x.CreateManyAsync(FakeDb.Tags));

            _mockQueueTagService.Setup(x => x.InitializeTagsAsync(FakeDb.Tags))
                .ReturnsAsync(FakeDb.QueueTags);
            //Act
            var result = await _controller.CreateAsync(queueDto);

            //Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task Create_DelayedQueueWithTagsAndImage_RedirectToActionHome()
        {
            //Arrange
            _fixture.Register<IFormFile>(() => null);

            var queueDto = _fixture.Build<CreateQueueDto>()
                .With(prop => prop.IsDelayed, true)   
                .Create();

            _mockMapper.Setup(x => x.Map<Queue>(It.IsAny<CreateQueueDto>()))
                .Returns(FakeDb.Queues.First());

            _mockQueueTypeService.Setup(x => x.GetQueueTypeAsync(It.IsAny<TypeName>()))
                .ReturnsAsync(FakeDb.QueueTypes.Where(type => type.Name.Equals(TypeName.Delayed)).First());

            _mockQueueService.Setup(option => option.CreateAsync(It.IsAny<Queue>()));

            _mockUserInQueueService.Setup(x => x.CreateDelayedPlaces(
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(FakeDb.UserInQueuesDelayed);

            _mockMapper.Setup(x => x.Map<List<Tag>>(It.IsAny<IList<string>>()))
                .Returns(FakeDb.Tags);

            _mockTagService.Setup(x => x.CreateManyAsync(FakeDb.Tags));

            _mockQueueTagService.Setup(x => x.InitializeTagsAsync(FakeDb.Tags))
                .ReturnsAsync(FakeDb.QueueTags);
            //Act
            var result = await _controller.CreateAsync(queueDto);

            //Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task Create_Queue_QueueDtoIsNull()
        {
            //Arrange
            var queueDto = new CreateQueueDto();

            _controller.ModelState.AddModelError(keyError, messageError);

            //Act
            var result = await _controller.CreateAsync(queueDto);
            
            //Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
