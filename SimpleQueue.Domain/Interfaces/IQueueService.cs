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
        Task CreateAsync(Queue queue);
        Task<Queue?> GetAsync(Guid id);
        Task<List<Queue>> GetAllOwnerQueuesAsync(Guid userId);
        Task<List<Queue>> GetAllParticipantQueuesAsync(Guid userId);
        Task FreezeAsync(Guid id);
        Task<UserInQueue?> NextParticipantAsync(Guid id);
        void DeleteParticipant(UserInQueue participant);
        void Delete(Queue queue);
        Task<PagedList<Queue>> GetQueuesAsync(QueueParameters queueParameters, bool trackChanges = true);
    }
}
