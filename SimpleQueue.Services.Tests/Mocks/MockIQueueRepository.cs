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
    internal class MockIQueueRepository
    {
        public static Mock<IQueueRepository> GetMock()
        {
            var mock = new Mock<IQueueRepository>();

            var queues = new List<Queue>()
            {
                new Queue
                {
                    Id = Guid.Parse("f2bd3543-ebbc-4b41-a4ed-5be0c87c5c2e"),
                    Title = "Frozen queue",
                    Description = "Description",
                    Chat = true,
                    IsFrozen = true,
                },
                new Queue
                {
                    Id = Guid.Parse("920cc596-e573-4cfd-89f8-94ac18290d10"),
                    Title = "Not frozen queue",
                    Description = "Description",
                    Chat = true,
                    IsFrozen = false,
                }
            };

            mock.Setup(m => m.CreateQueue(It.IsAny<Queue>()));
            mock.Setup(m => m.GetQueueAsync(It.IsAny<Guid>()))
                .Returns((Guid id) => Task.FromResult(queues.FirstOrDefault(o => o.Id.Equals(id))));

            return mock;
        }
    }
}
