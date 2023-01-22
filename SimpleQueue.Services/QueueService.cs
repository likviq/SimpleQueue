using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;

namespace SimpleQueue.Services
{
    public class QueueService : IQueueService
    {
        private readonly IRepositoryManager _repository;
        public QueueService(IRepositoryManager repository)
        {
            _repository = repository;
        }
        public void CreateQueue(Queue queue)
        {
            _repository.Queue.CreateQueue(queue);
            _repository.Save();
        }
    }
}
