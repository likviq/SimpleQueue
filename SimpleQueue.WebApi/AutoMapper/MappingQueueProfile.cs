using AutoMapper;
using SimpleQueue.Domain.Entities;
using SimpleQueue.WebApi.Models.ViewModels;

namespace SimpleQueue.WebApi.AutoMapper
{
    public class MappingQueueProfile: Profile
    {
        public MappingQueueProfile()
        {
            CreateMap<Queue, QueueSearchResultViewModel>()
                .ForMember(view => view.ImageLink,
                opt => opt.MapFrom(queue => queue.ImageBlob.ImageLink));
        }
    }
}
