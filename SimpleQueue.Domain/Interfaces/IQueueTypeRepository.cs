using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IQueueTypeRepository
    {
        Task<QueueType?> GetQueueAsync(TypeName typeName);
    }
}
