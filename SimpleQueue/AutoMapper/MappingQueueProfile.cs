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

            CreateMap<UserInQueue, UserInQueueViewModel>()
                .ForMember(c => c.Id,
                opt => opt.MapFrom(x => x.User.Id))
                .ForMember(c => c.Username,
                opt => opt.MapFrom(x => x.User.Username))
                .ForMember(c => c.Name,
                opt => opt.MapFrom(x => x.User.Name))
                .ForMember(c => c.Surname,
                opt => opt.MapFrom(x => x.User.Surname))
                .ForMember(c => c.JoinTime,
                opt => opt.MapFrom(x => x.JoinTime))
                .ForMember(c => c.IdInQueue,
                opt => opt.MapFrom(x => x.Id))
                .ForMember(c => c.NextIdInQueue,
                opt => opt.MapFrom(x => x.NextId));

            CreateMap<Queue, GetQueueViewModel>()
                .ForMember(view => view.Users,
                opt => opt.MapFrom(queue => queue.UserInQueues));
        }
    }
}
