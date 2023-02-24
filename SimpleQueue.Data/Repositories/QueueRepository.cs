using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SimpleQueue.Domain.RequestFeatures;
using SimpleQueue.Data.Extensions;

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
            await FindByCondition(q => q.Id.Equals(id))
            .Include(q => q.UserInQueues.OrderBy(userInQueue => userInQueue.JoinTime))
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync();

        public async Task<List<Queue>> GetOwnerQueuesAsync(Guid userId) => 
            await FindByCondition(x => x.OwnerId.Equals(userId))
            .ToListAsync();

        public async Task<List<Queue>> GetParticipantQueues(Guid userId) =>
            await FindAll().Include(item => item.UserInQueues)
            .Where(x => x.UserInQueues.Any(v => v.UserId.Equals(userId)))
            .ToListAsync();

        public void DeleteQueue(Queue queue) => Delete(queue);

        public async Task<PagedList<Queue>> GetQueuesAsync(QueueParameters queueParameters, bool trackChanges = true)
        {
            var queues = await FindAll().Include(queue => queue.UserInQueues)
                .FilterQueuesByTime(queueParameters.StartTime, queueParameters.EndTime)
                .FilterQueuesByFrozen(queueParameters.IsFrozen)
                .FilterQueuesByChat(queueParameters.IsChat)
                .FilterQueuesByPassword(queueParameters.IsProtected)
                .Search(queueParameters.SearchTerm)
                .SortBy(queueParameters.SortBy)
                .ToListAsync();

            return PagedList<Queue>.ToPagedList(
                queues, queueParameters.PageNumber, queueParameters.PageSize);
        }
    }
}
