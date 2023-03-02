using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserAsync(User user);
    }
}
