using Microsoft.EntityFrameworkCore;
using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;

namespace SimpleQueue.Data.Repositories
{
    public class QueueTypeRepository : RepositoryBase<QueueType>, IQueueTypeRepository
    {
        public QueueTypeRepository(SimpleQueueDBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public Task<QueueType?> GetQueueAsync(TypeName typeName) => 
            FindByCondition(queueType => queueType.Name.Equals(typeName))
            .FirstOrDefaultAsync();
    }
}
