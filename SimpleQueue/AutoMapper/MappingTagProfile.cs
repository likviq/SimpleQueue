using AutoMapper;
using SimpleQueue.Domain.Entities;

namespace SimpleQueue.WebUI.AutoMapper
{
    public class MappingTagProfile: Profile
    {
        public MappingTagProfile()
        {
            CreateMap<string, Tag>()
                .ForMember(c => c.TagTitle,
                opt => opt.MapFrom(value => value));
        }
    }
}
