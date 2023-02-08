using AutoMapper;
using SimpleQueue.Domain.Entities;
using SimpleQueue.IdentityServer.Controllers;

namespace SimpleQueue.IdentityServer.AutoMapper
{
    public class MappingUserProfile : Profile
    {
        public MappingUserProfile()
        {
            CreateMap<RegisterViewModel, User>()
                .ForMember(c => c.Name,
                opt => opt.MapFrom(x => x.FirstName));
        }
    }
}
