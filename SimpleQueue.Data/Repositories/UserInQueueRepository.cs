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

        public Task<UserInQueue?> FirstParticipantAsync(Guid queueId) =>
            FindByCondition(x => x.QueueId.Equals(queueId) && x.PreviousId == null)
            .FirstOrDefaultAsync();

        public void DeleteUserInQueue(UserInQueue userInQueue) =>
            Delete(userInQueue);

        public Task<UserInQueue?> GetAsync(Guid? userInQueueId) =>
            FindByCondition(x => x.Id.Equals(userInQueueId))
            .FirstOrDefaultAsync();

        public void CreateUserInQueue(UserInQueue userInQueue) =>
            Create(userInQueue);

        public async Task<bool> IsUserInQueueAsync(Guid userId, Guid queueId) =>
            await FindByCondition(x => x.UserId.Equals(userId) && x.QueueId.Equals(queueId))
            .FirstOrDefaultAsync() != null;

        public async Task<bool> IsDestinationInQueueAsync(Guid queueId, Guid userInQueueId) =>
            await FindByCondition(participant => participant.QueueId.Equals(queueId)
            && participant.Id.Equals(userInQueueId)
            && participant.UserId.Equals(null))
            .FirstOrDefaultAsync() != null;

        public Task<UserInQueue?> LastParticipantInQueueAsync(Guid queueId) =>
            FindByCondition(x => x.NextId == null && x.QueueId.Equals(queueId))
            .FirstOrDefaultAsync();
    }
}
