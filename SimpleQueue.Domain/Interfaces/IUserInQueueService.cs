using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IUserInQueueService
    {
        Task<List<UserInQueue>> GetQueueAsync(Guid id);
    }
}
