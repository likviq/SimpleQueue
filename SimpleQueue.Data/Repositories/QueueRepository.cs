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
        public async Task<Queue> GetQueueAsync(Guid id) =>
            await FindByCondition(x => x.Id.Equals(id))
            .Include(x => x.UserInQueues).ThenInclude(x => x.User)
            .FirstOrDefaultAsync();
    }
}
