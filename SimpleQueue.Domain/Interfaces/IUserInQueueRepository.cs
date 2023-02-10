using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IUserInQueueRepository
    {
        Task<UserInQueue?> FirstParticipant(Guid queueId);
        void DeleteUserInQueue(UserInQueue userInQueue);
    }
}
