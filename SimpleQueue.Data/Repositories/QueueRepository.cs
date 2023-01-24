using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SimpleQueue.Data.Repositories
{
    public class QueueRepository : EFRepositoryBase<Queue>, IQueueRepository
    {
        public QueueRepository(SimpleQueueDBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateQueue(Queue queue) => Create(queue);

        public async Task<Queue> GetQueueAsync(Guid queueId, bool trackChanges = true)
        {
            var queues = await FindByCondition(c => c.Id.Equals(queueId), trackChanges)
            .Include(item => item.UserInQueues)
            .ThenInclude(item => item.User).ToListAsync();

            return queues.SingleOrDefault();
        }
            
    }
}
