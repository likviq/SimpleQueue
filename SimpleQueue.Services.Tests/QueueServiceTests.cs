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
        public async Task GivenAnIdOfAnExistingQueue_WhenGettingQueueById_ThenQueueReturns()
        {
            var id = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e");
            
            var result = await _mockQueueService.GetQueue(id);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<Queue>(result);
            Assert.NotNull(result as Queue);
        }

        [Fact]
        public async Task GivenAnExistingQueue_WhenGettingQueueById_ThenQueueReturns()
        {
            var queue = _fixture.Build<Queue>().Without(p => p.UserInQueues).Create();

            var result = _mockQueueService.CreateQueue(queue);

            Assert.NotNull(result);
        }
    }
}
