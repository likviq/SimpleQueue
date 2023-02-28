using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IUserInQueueService
    {
        Task<UserInQueue?> GetUserInQueue(Guid userInQueueId);
        void Delete(UserInQueue userInQueue);
        void EnterQueue(UserInQueue userInQueue);
        bool IsUserInQueue(Guid userId, Guid queueId);
        bool IsDestinationInQueue(Guid queueId, Guid userInQueueId);
        Task<int?> UserPositionInQueue(Guid userId, Guid queueId);
        Task<UserInQueue> InitializeUserInQueue(Guid userId, Guid queueId);
        Task<UserInQueue> SetUserWithDestination(Guid userId, Guid userInQueueId);
        void MoveUserInQueueAfter(UserInQueue userInQueue, UserInQueue targetUserInQueue);
        List<UserInQueue> CreateDelayedPlaces(DateTime from, DateTime to, int durationInMinutes);
    }
}
