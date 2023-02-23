using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;

namespace SimpleQueue.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;
        public UserService(IRepositoryManager repository)
        {
            _repository = repository;
        }
        public Task RegisterUser(User user)
        {
            _repository.User.CreateUser(user);
            _repository.Save();

            return Task.CompletedTask;
        }
    }
}
