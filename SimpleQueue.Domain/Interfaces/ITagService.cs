using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface ITagService
    {
        Task<Tag?> GetTagAsync(string title);
        Task<Tag?> GetTagAsync(Guid id);
        Task CreateTagsAsync(List<Tag> tags);
    }
}
