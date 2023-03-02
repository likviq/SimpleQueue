using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface ITagRepository
    {
        void CreateTag(Tag tag);
        void CreateTags(List<Tag> tags);
        Task<Tag?> GetTagAsync(string title);
        Task<Tag?> GetTagAsync(Guid id);
    }
}
