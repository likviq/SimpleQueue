using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SimpleQueue.Domain.RequestFeatures;
using SimpleQueue.Data.Extensions;

namespace SimpleQueue.Data.Repositories
{
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(SimpleQueueDBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateTag(Tag tag) => Create(tag);

        public void CreateTags(List<Tag> tags) => CreateMany(tags);

        public Task<Tag?> GetTagAsync(string title) => 
            FindByCondition(tag => tag.TagTitle.Equals(title))
            .FirstOrDefaultAsync();

        public Task<Tag?> GetTagAsync(Guid id) =>
            FindByCondition(tag => tag.Id.Equals(id))
            .FirstOrDefaultAsync();
    }
}
