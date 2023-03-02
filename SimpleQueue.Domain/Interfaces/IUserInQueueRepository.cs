using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IUserInQueueRepository
    {
        Task<UserInQueue?> FirstParticipantAsync(Guid queueId);
        void DeleteUserInQueue(UserInQueue userInQueue);
        Task<UserInQueue?> GetAsync(Guid? userInQueueId);
        void CreateUserInQueue(UserInQueue userInQueue);
        Task<bool> IsUserInQueueAsync(Guid userId, Guid queueId);
        Task<bool> IsDestinationInQueueAsync(Guid queueId, Guid userInQueueId);
        Task<UserInQueue?> LastParticipantInQueueAsync(Guid queueId);
    }
}
