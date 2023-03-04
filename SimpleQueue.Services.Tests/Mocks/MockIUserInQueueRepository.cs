using AutoFixture;
using Moq;
using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleQueue.Services.Tests.Mocks
{
    internal class MockIUserInQueueRepository
    {
        public static Mock<IUserInQueueRepository> GetMock()
        {
            var mock = new Mock<IUserInQueueRepository>();

            var userInQueues = new List<UserInQueue>()
            {
                new UserInQueue
                {
                    Id = Guid.Parse("33e6241b-89ec-4a1a-b799-0da2db1b59a3"),
                    QueueId = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"),
                    UserId = Guid.Parse("35d05844-ea2e-460a-8207-d6601a3ced9b"),
                    NextId = Guid.Parse("14f4d214-7edb-4120-9b04-ed608ddabecd")
                },
                new UserInQueue
                {
                    Id = Guid.Parse("14f4d214-7edb-4120-9b04-ed608ddabecd"),
                    QueueId = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"),
                    UserId = Guid.Parse("33112f17-5f3e-4c38-b90b-630a8d3b0e18"),
                    NextId = null
                },
                new UserInQueue
                {
                    Id = Guid.Parse("14f4d214-7edb-4120-9b04-ed608ddabecd"),
                    QueueId = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"),
                    UserId = null,
                    NextId = null
                }
            };

            mock.Setup(m => m.CreateUserInQueue(It.IsAny<UserInQueue>()));
            mock.Setup(m => m.GetAsync(It.IsAny<Guid>()))
                .Returns((Guid id) => Task.FromResult(userInQueues.FirstOrDefault(o => o.Id.Equals(id))));
            mock.Setup(m => m.FirstParticipantAsync(It.IsAny<Guid>()))
                .Returns((Guid id) => Task.FromResult(userInQueues.FirstOrDefault(o => o.QueueId.Equals(id))));
            mock.Setup(m => m.LastParticipantInQueueAsync(It.IsAny<Guid>()))
                .Returns((Guid id) => Task.FromResult(userInQueues.LastOrDefault(o => o.QueueId.Equals(id))));

            return mock;
        }
    }
}
