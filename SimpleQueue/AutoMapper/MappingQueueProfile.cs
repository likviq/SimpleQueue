using AutoMapper;
using SimpleQueue.WebUI.Models.DataTransferObjects;
using SimpleQueue.Domain.Entities;
using SimpleQueue.WebUI.Models.ViewModels;

namespace SimpleQueue.WebUI.Automapper
{
    public class MappingQueueProfile : Profile
    {
        public MappingQueueProfile()
        {
            CreateMap<CreateQueueDto, Queue>()
                .ForMember(c => c.Chat,
                opt => opt.MapFrom(x => x.IsChat));

            CreateMap<Queue, GetQueueViewModel>();
        }
    }
}
