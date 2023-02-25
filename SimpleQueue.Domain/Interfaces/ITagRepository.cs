using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface ITagRepository
    {
        void CreateTag(Tag tag);
        void CreateTags(List<Tag> tags);
        Task<Tag?> GetTag(string title);
        Task<Tag?> GetTag(Guid id);
    }
}
