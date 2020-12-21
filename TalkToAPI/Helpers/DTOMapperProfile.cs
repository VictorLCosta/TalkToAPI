using AutoMapper;
using TalkToAPI.V1.Models;
using TalkToAPI.V1.Models.DTO;

namespace TalkToAPI.Helpers
{
    public class DTOMapperProfile : Profile
    {
        public DTOMapperProfile()
        {
            CreateMap<ApplicationUser, DTOUser>()
                .ForMember(p => p.Name, o => o.MapFrom(src => src.FullName));

            CreateMap<Message, DTOMessage>();
        }
    }
}