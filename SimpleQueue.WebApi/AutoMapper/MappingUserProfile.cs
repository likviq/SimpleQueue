using AutoMapper;
using SimpleQueue.Domain.Entities;
using SimpleQueue.WebApi.Models.ViewModels;

namespace SimpleQueue.WebApi.AutoMapper
{
    public class MappingUserProfile : Profile
    {
        public MappingUserProfile()
        {
            CreateMap<UserInQueue, UserInQueueViewModel>()
                .ForMember(c => c.UserInQueueId,
                opt => opt.MapFrom(x => x.Id));
        }
    }
}
