using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IQueueRepository
    {
        void CreateQueue(Queue queue);
        Task<Queue> GetQueueAsync(Guid queueId, bool trackChanges = true);
    }
}
