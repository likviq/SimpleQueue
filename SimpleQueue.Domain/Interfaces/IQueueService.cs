using SimpleQueue.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IQueueService
    {
        Task CreateQueue(Queue queue);
        Task<Queue> GetQueue(Guid id);
        Task<List<Queue>> GetAllOwnerQueues(Guid userId);
        Task<List<Queue>> GetAllParticipantQueues(Guid userId);
    }
}
