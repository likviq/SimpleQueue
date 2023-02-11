using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IUserInQueueService
    {
        Task<UserInQueue?> GetUserInQueue(Guid userInQueueId);
        void Delete(UserInQueue userInQueue);
        void EnterQueue(UserInQueue userInQueue);
        bool IsUserInQueue(Guid userId, Guid queueId);
        Task<UserInQueue> InitializeUserInQueue(Guid userId, Guid queueId);
    }
}
