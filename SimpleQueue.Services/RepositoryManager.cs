using SimpleQueue.Data;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleQueue.Services
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

        public void Save() => _context.SaveChanges();
    }
}
