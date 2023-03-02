using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IUserInQueueService
    {
        Task<UserInQueue?> GetUserInQueueAsync(Guid userInQueueId);
        void Delete(UserInQueue userInQueue);
        void EnterQueue(UserInQueue userInQueue);
        Task<bool> IsUserInQueueAsync(Guid userId, Guid queueId);
        Task<bool> IsDestinationInQueueAsync(Guid queueId, Guid userInQueueId);
        Task<int?> UserPositionInQueueAsync(Guid userId, Guid queueId);
        Task<UserInQueue> InitializeUserInQueueAsync(Guid userId, Guid queueId);
        Task<UserInQueue> SetUserWithDestinationAsync(Guid userId, Guid userInQueueId);
        void MoveUserInQueueAfter(UserInQueue userInQueue, UserInQueue targetUserInQueue);
        List<UserInQueue> CreateDelayedPlaces(DateTime from, DateTime to, int durationInMinutes);
    }
}
