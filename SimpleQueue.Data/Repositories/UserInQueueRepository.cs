using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SimpleQueue.Data.Repositories
{
    public class UserInQueueRepository: EFRepositoryBase<UserInQueue>, IUserInQueueRepository
    {
        public UserInQueueRepository(DbContext context)
            : base(context)
        {
        }

        public async Task<List<UserInQueue>> GetQueueAsync(Guid queueId, bool trackChanges = true) =>
            await FindByCondition(c => c.QueueId.Equals(queueId), false)
            
            .ToListAsync();
    }
}
