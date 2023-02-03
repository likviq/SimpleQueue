using SimpleQueue.Data;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SimpleQueue.Data.Repositories
{
    public class QueueRepository : RepositoryBase<Queue>, IQueueRepository
    {
        public QueueRepository(SimpleQueueDBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateQueue(Queue queue) => Create(queue);

        public async Task<Queue?> GetQueueAsync(Guid id) =>
            await FindByCondition(x => x.Id.Equals(id))
            .Include(x => x.UserInQueues).ThenInclude(x => x.User)
            .FirstOrDefaultAsync();

        public async Task<List<Queue>> GetOwnerQueuesAsync(Guid userId) => 
            await FindByCondition(x => x.OwnerId.Equals(userId))
            .ToListAsync();

        public async Task<List<Queue>> GetParticipantQueues(Guid userId) =>
            await FindAll().Include(item => item.UserInQueues)
            .Where(x => x.UserInQueues.Any(v => v.UserId.Equals(userId)))
            .ToListAsync();
    }
}
