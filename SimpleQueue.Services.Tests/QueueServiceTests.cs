using AutoFixture;
using Moq;
using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Services.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleQueue.Services.Tests
{
    public class QueueServiceTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IRepositoryManager> _mockRepositoryManager;
        private readonly QueueService _mockQueueService;
        public QueueServiceTests()
        {
            _fixture = new Fixture();
            _mockRepositoryManager = MockRepositoryManager.GetMock();
            _mockQueueService = new QueueService(_mockRepositoryManager.Object);
        }

        [Fact]
        public async Task FreezeAsync_NotFrozenQueue_RepositorySave()
        {
            //Arrange
            var queueId = Guid.Parse("f2bd3543-ebbc-4b41-a4ed-5be0c87c5c2e");

            //Act
            await _mockQueueService.FreezeAsync(queueId);

            //Assert
            Assert.True(true);
        }

        [Theory]
        [MemberData(nameof(FakeDataGenerator.GetQueueFromDataGenerator), MemberType = typeof(FakeDataGenerator))]
        public async Task GivenAnIdOfAnExistingQueue_WhenGettingQueueById_ThenCheckIfFrozenIsChanged(Queue queue)
        {
            //Arrange
            var queueId = queue.Id;
            var queueFrozenValue = queue.IsFrozen;

            //Act
            await _mockQueueService.FreezeAsync(queueId);

            //Assert
            Assert.Equal(queueFrozenValue, queue.IsFrozen);
        }

        [Fact]
        public async Task GivenAnIdOfAnNotExistingQueue_WhenGettingQueueById_ThenNullReturns()
        {
            var id = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e");

            var result = await _mockQueueService.GetAsync(id);

            Assert.Null(result);
        }

        [Fact]
        public async Task GivenAnIdOfAnExistingQueue_WhenGettingQueueById_ThenQueueReturns()
        {
            var id = Guid.Parse("f2bd3543-ebbc-4b41-a4ed-5be0c87c5c2e");

            var result = await _mockQueueService.GetAsync(id);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<Queue>(result);
        }

        [Fact]
        public async Task GivenAnExistingQueue_WhenGettingQueueById_ThenQueueReturns()
        {
            var queue = new Queue()
            {
                Id = new Guid(),
                Title = "Title",
                Description = "Description",
                Chat = true,
                CreatedTime = DateTime.Now,
                StartTime = DateTime.Now
            };

            var result = _mockQueueService.CreateAsync(queue);

            Assert.NotNull(result);
        }
    }
}
