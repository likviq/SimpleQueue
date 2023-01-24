using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IUserInQueueRepository
    {
        Task<List<UserInQueue>> GetQueueAsync(Guid queueId, bool trackChanges = true);
    }
}
