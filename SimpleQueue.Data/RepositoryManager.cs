using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Data.Repositories;

namespace SimpleQueue.Data
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly SimpleQueueDBContext _context;
        private IUserRepository _userRepository;
        private IQueueRepository _queueRepository;
        private IUserInQueueRepository _userInQueueRepository;
        private ITagRepository _tagRepository;
        private IQueueTypeRepository _queueTypeRepository;

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

        public IUserInQueueRepository UserInQueue
        {
            get
            {
                if (_userInQueueRepository == null)
                    _userInQueueRepository = new UserInQueueRepository(_context);
                return _userInQueueRepository;
            }
        }

        public ITagRepository Tag
        {
            get
            {
                if (_tagRepository == null)
                    _tagRepository = new TagRepository(_context);
                return _tagRepository;
            }
        }

        public IQueueTypeRepository QueueType
        {
            get
            {
                if (_queueTypeRepository == null)
                    _queueTypeRepository = new QueueTypeRepository(_context);
                return _queueTypeRepository;
            }
        }

        public void Save() => _context.SaveChanges();
    }
}
