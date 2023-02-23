using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.RequestFeatures;
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
        Task FreezeQueue(Guid id);
        Task<UserInQueue?> NextParticipant(Guid id);
        void DeleteParticipant(UserInQueue participant);
        void DeleteQueue(Queue queue);
        Task<PagedList<Queue>> GetQueuesAsync(QueueParameters queueParameters, bool trackChanges = true);
    }
}
