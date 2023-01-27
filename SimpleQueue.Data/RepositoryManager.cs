using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Data.Repositories;

namespace SimpleQueue.Data
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly SimpleQueueDBContext _context;
        private IUserRepository _userRepository;
        private IQueueRepository _queueRepository;

        public RepositoryManager(SimpleQueueDBContext context)
        {
            _context = context;
        }

        public IUserRepository User
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_context);
                return _userRepository;
            }
        }

        public IQueueRepository Queue
        {
            get
            {
                if (_queueRepository == null)
                    _queueRepository = new QueueRepository(_context);
                return _queueRepository;
            }
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
