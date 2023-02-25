using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface ITagService
    {
        Task<Tag?> GetTag(string title);
        Task<Tag?> GetTag(Guid id);
        Task CreateTags(List<Tag> tags);
    }
}
