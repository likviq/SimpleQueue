using AutoFixture;
using Moq;
using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Services.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleQueue.Services.Tests
{
    public class UserInQueueServiceTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IRepositoryManager> _mockRepositoryManager;
        private readonly UserInQueueService _mockUserInQueueService;
        public UserInQueueServiceTests()
        {
            _fixture = new Fixture();
            _mockRepositoryManager = MockRepositoryManager.GetMock();
            _mockUserInQueueService = new UserInQueueService(_mockRepositoryManager.Object);
        }

        [Fact]
        public async Task UserPositionInQueueAsync_FirstParticipant_ReturnsParticipantPosition()
        {
            //Arrange
            var queueId = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e");
            var userId = Guid.Parse("35d05844-ea2e-460a-8207-d6601a3ced9b");

            //Act
            var result = await _mockUserInQueueService.UserPositionInQueueAsync(userId, queueId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<int>(result);
            Assert.Equal(result, 1);
        }

        [Fact]
        public async Task UserPositionInQueueAsync_SecondParticipant_ReturnsParticipantPosition()
        {
            //Arrange
            var queueId = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e");
            var userId = Guid.Parse("33112f17-5f3e-4c38-b90b-630a8d3b0e18");

            //Act
            var result = await _mockUserInQueueService.UserPositionInQueueAsync(userId, queueId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<int>(result);
            Assert.Equal(result, 2);
        }

        [Theory]
        [MemberData(nameof(FakeDataGenerator.GetUserIdAndQueueIdFromDataGenerator), MemberType = typeof(FakeDataGenerator))]

        public async Task UserPositionInQueueAsync_UserNotInQueue_ReturnsNull(Guid userId, Guid queueId)
        {
            //Arrange

            //Act
            var result = await _mockUserInQueueService.UserPositionInQueueAsync(userId, queueId);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SetUserWithDestinationAsync_UserIdAndParticipantId_ReturnsParticipant()
        {
            //Arrange
            var userId = Guid.Parse("35d05844-ea2e-460a-8207-d6601a3ced9b");
            var participantId = Guid.Parse("14f4d214-7edb-4120-9b04-ed608ddabecd");

            //Act
            var result = await _mockUserInQueueService.SetUserWithDestinationAsync(userId, participantId);

            //Assert
            Assert.IsType<UserInQueue>(result);
            Assert.NotNull(result.UserId);
        }

        [Fact]
        public void CreateDelayedPlaces_FromToDateTime_ReturnsParticipantsList()
        {
            //Arrange
            DateTime from = new DateTime(2023, 3, 4, 10, 0, 0);
            DateTime to = from.AddHours(5);
            int durationInMinutes = 30;

            //Act
            var result = _mockUserInQueueService.CreateDelayedPlaces(from, to, durationInMinutes);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(10, result.Count);
            Assert.IsType<List<UserInQueue>>(result);
        }

        [Fact]
        public void CreateDelayedPlaces_FromToDateTimeIsEqual_ReturnsParticipantsList()
        {
            //Arrange
            DateTime from = new DateTime(2023, 3, 4, 10, 0, 0);
            DateTime to = from;
            int durationInMinutes = 30;

            //Act
            var result = _mockUserInQueueService.CreateDelayedPlaces(from, to, durationInMinutes);

            //Assert
            Assert.False(result.Any());
            Assert.IsType<List<UserInQueue>>(result);
        }

        [Theory]
        [MemberData(nameof(FakeDataGenerator.InitializeUserInQueueAsyncDataGenerator), MemberType = typeof(FakeDataGenerator))]
        public async Task InitializeUserInQueueAsync_QueueHasParticipantsUserIdAndQueueId_ReturnsUserInQueueObject(Guid userId, Guid queueId)
        {
            //Arrange

            //Act
            var result = await _mockUserInQueueService.InitializeUserInQueueAsync(userId, queueId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<UserInQueue>(result);
            Assert.Equal(userId, result.UserId);
        }
    }
}
