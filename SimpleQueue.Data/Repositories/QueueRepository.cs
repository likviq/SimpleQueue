using SimpleQueue.Data;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SimpleQueue.Data.Repositories
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public Guid IdInQueue { get; set; }
        public string UserName { get; set; }
        public DateTime JoinTime { get; set; }
    }

    public class QueueDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
       = Array.Empty<UserDto>();
    }

    public class QueueRepository : EFRepositoryBase<Queue>, IQueueRepository
    {
        public QueueRepository(SimpleQueueDBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateQueue(Queue queue) => Create(queue);
        public async Task<Queue> GetQueueAsync(Guid id) =>
            await FindByCondition(x => x.Id.Equals(id))
            .SingleOrDefaultAsync();
    }
}
