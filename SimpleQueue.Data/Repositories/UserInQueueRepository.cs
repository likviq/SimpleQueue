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
    }
}
