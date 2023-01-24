using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;

namespace SimpleQueue.Services
{
    public class UserInQueueService: IUserInQueueService
    {
        private readonly IRepositoryManager _repository;
        public UserInQueueService(IRepositoryManager repository)
        {
            _repository = repository;
        }
        public async Task<List<UserInQueue>> GetQueueAsync(Guid id)
        {
            return await _repository.UserInQueue.GetQueueAsync(id);
        }
    }
}
