using AutoMapper;
using SimpleQueue.WebUI.Models.DataTransferObjects;
using SimpleQueue.Domain.Entities;

namespace SimpleQueue.WebUI.Automapper
{
    public class MappingQueueProfile : Profile
    {
        public MappingQueueProfile()
        {
            CreateMap<CreateQueueDto, Queue>()
                .ForMember(c => c.Chat,
                opt => opt.MapFrom(x => x.IsChat));
        }
    }
}
