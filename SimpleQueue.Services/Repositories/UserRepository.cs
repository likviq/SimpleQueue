using SimpleQueue.Data;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Domain.Models;

namespace SimpleQueue.Services.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(SimpleQueueDBContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
