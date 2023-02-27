using Microsoft.EntityFrameworkCore;
using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;

namespace SimpleQueue.Data.Repositories
{
    public class UserInQueueRepository : RepositoryBase<UserInQueue>, IUserInQueueRepository
    {
        public UserInQueueRepository(SimpleQueueDBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<UserInQueue?> FirstParticipant(Guid queueId) =>
            await FindByCondition(x => x.QueueId.Equals(queueId) && x.PreviousId == null)
            .FirstOrDefaultAsync();

        public void DeleteUserInQueue(UserInQueue userInQueue) =>
            Delete(userInQueue);

        public async Task<UserInQueue?> Get(Guid? userInQueueId) =>
            await FindByCondition(x => x.Id.Equals(userInQueueId))
            .FirstOrDefaultAsync();

        public async Task<UserInQueue?> Get(Guid queueId, DateTime destinationTime) =>
            await FindByCondition(x => x.QueueId.Equals(queueId)
            && x.DestinationTime.Equals(destinationTime))
            .FirstOrDefaultAsync();

        public void CreateUserInQueue(UserInQueue userInQueue) =>
            Create(userInQueue);

        public bool IsUserInQueue(Guid userId, Guid queueId) =>
            FindByCondition(x => x.UserId.Equals(userId) && x.QueueId.Equals(queueId))
            .FirstOrDefault() != null;

        public bool IsDestinationInQueue(Guid queueId, DateTime destinationTime) =>
            FindByCondition(participant => participant.QueueId.Equals(queueId)
            && participant.DestinationTime.Equals(destinationTime)
            && participant.UserId.Equals(Guid.Empty))
            .FirstOrDefault() != null;

        public async Task<UserInQueue?> LastParticipantInQueue(Guid queueId) =>
            await FindByCondition(x => x.NextId == null && x.QueueId.Equals(queueId))
            .FirstOrDefaultAsync();
    }
}
