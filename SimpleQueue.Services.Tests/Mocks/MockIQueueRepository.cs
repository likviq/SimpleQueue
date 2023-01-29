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
                new Queue() 
                { 
                    Id = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"),
                    Title = "Title",
                    Description = "Description",
                    Chat = true,
                    CreatedTime = DateTime.Now,
                    StartTime = DateTime.Now
                }
            };

            mock.Setup(m => m.CreateQueue(It.IsAny<Queue>()));
            mock.Setup(m => m.GetQueueAsync(It.IsAny<Guid>()))
                .Returns((Guid id) => Task.FromResult(queues.FirstOrDefault(o => o.Id.Equals(id))));

            return mock;
        }
    }
}
