using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Domain.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleQueue.WebApi.Tests.Mocks
{
    public class FakeQueueService : IQueueService
    {
        private readonly List<Queue> _queue;
        public FakeQueueService()
        {
            _queue = new List<Queue>()
            {
                new Queue
                {
                    Id = Guid.Parse("f2bd3543-ebbc-4b41-a4ed-5be0c87c5c2e"),
                    Title = "FastQueue",
                    Description = "Description",
                    Chat = true,
                    IsFrozen = false,
                    OwnerId = Guid.Parse("9355a7df-c059-4c23-8c1f-31ec31962944")
                }
            };
        }
        public Task CreateAsync(Queue queue)
        {
            throw new NotImplementedException();
        }

        public void Delete(Queue queue)
        {
            throw new NotImplementedException();
        }

        public void DeleteParticipant(UserInQueue participant)
        {
            throw new NotImplementedException();
        }

        public Task FreezeAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Queue>> GetAllOwnerQueuesAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Queue>> GetAllParticipantQueuesAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Queue?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<Queue>> GetQueuesAsync(QueueParameters queueParameters, bool trackChanges = true)
        {
            throw new NotImplementedException();
        }

        public Task<UserInQueue?> NextParticipantAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
