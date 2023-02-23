using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IUserRepository
    {
        void CreateUser(User user);
    }
}
