using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IRepositoryManager
    {
        IUserRepository User { get; }
        IQueueRepository Queue { get; }
        IUserInQueueRepository UserInQueue { get; }
        ITagRepository Tag { get; }
        IQueueTypeRepository QueueType { get; }
        void Save();
    }
}
