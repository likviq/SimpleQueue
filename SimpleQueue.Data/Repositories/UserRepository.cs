using SimpleQueue.Data;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(SimpleQueueDBContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
