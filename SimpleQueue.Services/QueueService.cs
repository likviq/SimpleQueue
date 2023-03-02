using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Domain.RequestFeatures;

namespace SimpleQueue.Services
{
    public class QueueService : IQueueService
    {
        private readonly IRepositoryManager _repository;
        public QueueService(IRepositoryManager repository)
        {
            _repository = repository;
        }
        public Task CreateAsync(Queue queue)
        {
            _repository.Queue.CreateQueue(queue);
            _repository.Save();

            return Task.CompletedTask;
        }

        public Task<Queue?> GetAsync(Guid id)
        {
            return _repository.Queue.GetQueueAsync(id);
        }

        public Task<List<Queue>> GetAllOwnerQueuesAsync(Guid userId)
        {
            return _repository.Queue.GetOwnerQueuesAsync(userId);
        }

        public Task<List<Queue>> GetAllParticipantQueuesAsync(Guid userId)
        {
            return _repository.Queue.GetParticipantQueuesAsync(userId);
        }

        public async Task FreezeAsync(Guid id)
        {
            var queue = await _repository.Queue.GetQueueAsync(id);

            queue.IsFrozen = !queue.IsFrozen;

            _repository.Save();
        }

        public Task<UserInQueue?> NextParticipantAsync(Guid id)
        {
            return _repository.UserInQueue.FirstParticipantAsync(id);
        }

        public void DeleteParticipant(UserInQueue participant)
        {
            _repository.UserInQueue.DeleteUserInQueue(participant);
            _repository.Save();
        }

        public void Delete(Queue queue)
        {
            _repository.Queue.DeleteQueue(queue);
            _repository.Save();
        }

        public Task<PagedList<Queue>> GetQueuesAsync(QueueParameters queueParameters, bool trackChanges = true)
        {
            return _repository.Queue.GetQueuesAsync(queueParameters, trackChanges);
        }
    }
}
