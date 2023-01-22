using SimpleQueue.Data;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Data.Repositories
{
    public class QueueRepository : EFRepositoryBase<Queue>, IQueueRepository
    {
        public QueueRepository(SimpleQueueDBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateQueue(Queue queue) => Create(queue);
    }
}
