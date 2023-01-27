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

namespace SimpleQueue.WebUI.Tests.Controllers
{
    public class QueueControllerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IQueueService> _mockQueueService;
        private readonly Mock<ILoggerManager> _mockQogger;
        private readonly QueueController _controller;
        private readonly Fixture _fixture;

        const string keyError = "key";
        const string messageError = "value";

        public QueueControllerTests()
        {
            
            _mockMapper = new Mock<IMapper>();
            _mockQueueService = new Mock<IQueueService>();
            _mockQogger = new Mock<ILoggerManager>();
            _controller = new QueueController(_mockMapper.Object, _mockQueueService.Object, _mockQogger.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public void Create_ActionExecutes_ReturnsViewForCreate()
        {
            var result = _controller.Create();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_Queue_RedirectToActionHome()
        {
            var queueDto = _fixture.Create<CreateQueueDto>();

            _mockMapper.Setup(x => x.Map<Queue>(It.IsAny<CreateQueueDto>()))
                .Returns(It.IsAny<Queue>());

            _mockQueueService.Setup(option => option.CreateQueue(It.IsAny<Queue>()));
            
            var result = await _controller.CreateAsync(queueDto);

            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task Create_Queue_QueueDtoIsNull()
        {
            var queueDto = new CreateQueueDto();

            _controller.ModelState.AddModelError(keyError, messageError);

            var result = await _controller.CreateAsync(queueDto);

            Assert.IsType<ViewResult>(result);
        }

        //[Fact]
        //public async Task Create_Queue_ThrowException()
        //{
        //    var queueDto = _fixture.Create<CreateQueueDto>();

        //    _mockMapper.Setup(x => x.Map<Queue>(It.IsAny<CreateQueueDto>()))
        //        .Returns(It.IsAny<Queue>());

        //    _mockQueueService.Setup(option => option.CreateQueue(It.IsAny<Queue>())).Throws(new Exception("a"));

        //    var result = await _controller.CreateAsync(queueDto) as ObjectResult;

        //    Assert.IsType<InvalidOperationException>(result);
        //}

        [Fact]
        public async Task Get_QueueIdIsNull_ReturnsBadRequest()
        {
            _mockMapper.Setup(x => x.Map<GetQueueViewModel>(It.IsAny<Queue>()))
                .Returns(It.IsAny<GetQueueViewModel>());

            _mockQueueService.Setup(option => option.GetQueue(It.IsAny<Guid>()));

            var result = await _controller.GetAsync(It.IsAny<Guid>());

            Assert.IsType<BadRequestResult>(result);
        }
    }
}
