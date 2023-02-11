using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IQueueRepository
    {
        void CreateQueue(Queue queue);
        Task<Queue?> GetQueueAsync(Guid id);
        Task<List<Queue>> GetOwnerQueuesAsync(Guid userId);
        Task<List<Queue>> GetParticipantQueues(Guid userId);
        void DeleteQueue(Queue queue);
    }
}
