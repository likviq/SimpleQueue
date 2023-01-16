using AutoMapper;
using SimpleQueue.WebUI.Models.DataTransferObjects;
using SimpleQueue.Domain.Models;

namespace SimpleQueue.WebUI.Automapper
{
    public class MappingQueueProfile : Profile
    {
        public MappingQueueProfile()
        {
            CreateMap<QueueForCreationDto, Queue>();
        }
    }
}
