using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;

namespace SimpleQueue.Services
{
    public class QueueService : IQueueService
    {
        private readonly IRepositoryManager _repository;
        public QueueService(IRepositoryManager repository)
        {
            _repository = repository;
        }
        public async Task CreateQueue(Queue queue)
        {
            _repository.Queue.CreateQueue(queue);
            await _repository.SaveAsync();
        }

        public async Task<Queue> GetQueue(Guid id)
        {
            return await _repository.Queue.GetQueueAsync(id);
        }

        public async Task<List<Queue>> GetAllOwnerQueues(Guid userId)
        {
            var ownerQueues = await _repository.Queue.GetOwnerQueuesAsync(userId);

            return ownerQueues;
        }

        public async Task<List<Queue>> GetAllParticipantQueues(Guid userId)
        {
            var participantQueues = await _repository.Queue.GetParticipantQueues(userId);

            return participantQueues;
        }
    }
}
