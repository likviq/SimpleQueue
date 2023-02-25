using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;

namespace SimpleQueue.Services
{
    public class TagService : ITagService
    {
        private readonly IRepositoryManager _repository;
        public TagService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public Task<Tag?> GetTag(string title)
        {
            var tag = _repository.Tag.GetTag(title);
            return tag;
        }

        public Task<Tag?> GetTag(Guid id)
        {
            return _repository.Tag.GetTag(id);
        }

        public async Task CreateTags(List<Tag> tags)
        {
            var tagsList = new List<Tag>();

            foreach(var tag in tags)
            {
                var tagDb = await GetTag(tag.TagTitle);
                if (tagDb == null)
                {
                    tagsList.Add(tag);
                }
            }
            _repository.Tag.CreateTags(tagsList);
            _repository.Save();
        }
    }
}
