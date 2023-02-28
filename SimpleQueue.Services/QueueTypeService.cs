using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;

namespace SimpleQueue.Services
{
    public class QueueTypeService : IQueueTypeService
    {
        private readonly IRepositoryManager _repository;
        public QueueTypeService(IRepositoryManager repository)
        {
            _repository = repository;
        }
        public Task<QueueType?> GetQueueType(TypeName typeName)
        {
            return _repository.QueueType.GetQueueAsync(typeName);
        }
    }
}
