using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleQueue.Services
{
    public class QueueTagService : IQueueTagService
    {
        private readonly IRepositoryManager _repository;
        public QueueTagService(IRepositoryManager repository)
        {
            _repository = repository;
        }
        public async Task<List<QueueTag>> InitializeTagsAsync(List<Tag> tagTitles)
        {
            var queueTags = new List<QueueTag>();
            foreach (var tagTitle in tagTitles)
            {
                var tag = await _repository.Tag.GetTagAsync(tagTitle.TagTitle);

                var queueTagObject = new QueueTag(){
                    TagId = tag.Id
                };

                queueTags.Add(queueTagObject);
            }

            return queueTags;
        }
    }
}
