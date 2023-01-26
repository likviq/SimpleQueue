using SimpleQueue.Data;
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
        public async Task<Queue> GetQueueAsync(Guid id) =>
            await FindByCondition(x => x.Id.Equals(id))
            .Select(q => new Queue
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description,
                Chat = q.Chat,
                CreatedTime = q.CreatedTime,
                StartTime = q.StartTime,
                UserInQueues = q
                    .UserInQueues
                    .Select(u => new UserInQueue
                    {
                        Id = u.Id,
                        UserId = u.UserId,
                        NextId = u.NextId,
                        JoinTime = u.JoinTime,
                        User = u.User
                    }).ToList()
            })
            .FirstAsync();
    }
}
