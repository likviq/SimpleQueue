using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IUserService
    {
        Task RegisterUser(User user);
    }
}
