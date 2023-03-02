using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IQueueTypeService
    {
        Task<QueueType?> GetQueueTypeAsync(TypeName typeName);
    }
}
