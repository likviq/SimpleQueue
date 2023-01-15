using SimpleQueue.Data;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Domain.Models;

namespace SimpleQueue.Services.Repositories
{
    public class QueueRepository : RepositoryBase<Queue>, IQueueRepository
    {
        public QueueRepository(SimpleQueueDBContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
