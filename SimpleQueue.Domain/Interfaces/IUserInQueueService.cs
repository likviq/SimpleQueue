using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IUserInQueueService
    {
        Task<UserInQueue?> GetUserInQueue(Guid userInQueueId);
        Task<UserInQueue?> GetUserInQueue(Guid queueId, DateTime destinationTime);
        void Delete(UserInQueue userInQueue);
        void EnterQueue(UserInQueue userInQueue);
        bool IsUserInQueue(Guid userId, Guid queueId);
        bool IsDestinationInQueue(Guid queueId, DateTime destinationTime);
        Task<int?> UserPositionInQueue(Guid userId, Guid queueId);
        Task<UserInQueue> InitializeUserInQueue(Guid userId, Guid queueId);
        Task<UserInQueue> SetUserWithDestination(Guid queueId, Guid userId, DateTime destinationTime);
        void MoveUserInQueueAfter(UserInQueue userInQueue, UserInQueue targetUserInQueue);
        List<UserInQueue> CreateDelayedPlaces(DateTime from, DateTime to, int durationInMinutes);
    }
}
