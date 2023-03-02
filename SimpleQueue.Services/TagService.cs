﻿using SimpleQueue.Domain.Entities;
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

        public Task<Tag?> GetTagAsync(string title)
        {
            return _repository.Tag.GetTagAsync(title);
        }

        public Task<Tag?> GetTagAsync(Guid id)
        {
            return _repository.Tag.GetTagAsync(id);
        }

        public async Task CreateTagsAsync(List<Tag> tags)
        {
            var tagsList = new List<Tag>();

            foreach(var tag in tags)
            {
                var tagDb = await GetTagAsync(tag.TagTitle);
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
