using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface ITagService
    {
        Task<Tag?> GetAsync(string title);
        Task<Tag?> GetAsync(Guid id);
        Task CreateManyAsync(List<Tag> tags);
    }
}
