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
        public Task CreateQueue(Queue queue)
        {
            _repository.Queue.CreateQueue(queue);
            _repository.Save();

            return Task.CompletedTask;
        }

        public Task<Queue?> GetQueue(Guid id)
        {
            return _repository.Queue.GetQueueAsync(id);
        }

        public Task<List<Queue>> GetAllOwnerQueues(Guid userId)
        {
            return _repository.Queue.GetOwnerQueuesAsync(userId);
        }

        public Task<List<Queue>> GetAllParticipantQueues(Guid userId)
        {
            return _repository.Queue.GetParticipantQueues(userId);
        }

        public async Task FreezeQueue(Guid id)
        {
            var queue = await _repository.Queue.GetQueueAsync(id);

            queue.IsFrozen = !queue.IsFrozen;

            _repository.Save();
        }

        public Task<UserInQueue?> NextParticipant(Guid id)
        {
            return _repository.UserInQueue.FirstParticipant(id);
        }

        public void DeleteParticipant(UserInQueue participant)
        {
            _repository.UserInQueue.DeleteUserInQueue(participant);
            _repository.Save();
        }

        public void DeleteQueue(Queue queue)
        {
            _repository.Queue.DeleteQueue(queue);
            _repository.Save();
        }
    }
}
