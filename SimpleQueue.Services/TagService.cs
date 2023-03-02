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

        public Task<Tag?> GetAsync(string title)
        {
            return _repository.Tag.GetTagAsync(title);
        }

        public Task<Tag?> GetAsync(Guid id)
        {
            return _repository.Tag.GetTagAsync(id);
        }

        public async Task CreateManyAsync(List<Tag> tags)
        {
            var tagsList = new List<Tag>();

            foreach(var tag in tags)
            {
                var tagDb = await GetAsync(tag.TagTitle);
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
