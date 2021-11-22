using AutoMapper;
using ECO.Sample.Application.Events.DTO;
using ECO.Sample.Application.Speakers.DTO;
using ECO.Sample.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Utils
{
    public class DTOsProfile : Profile
    {
        public DTOsProfile()
        {
            CreateMap<Event, EventItem>()
                .ForCtorParam("EventCode", opt => opt.MapFrom(src => src.Identity))
                .ForCtorParam("StartDate", opt => opt.MapFrom(src => src.Period.StartDate))
                .ForCtorParam("EndDate", opt => opt.MapFrom(src => src.Period.EndDate))
                .ForCtorParam("NumberOfSessions", opt => opt.MapFrom(src => src.Sessions.Count()));
            CreateMap<Event, EventDetail>()
                .ForCtorParam("EventCode", opt => opt.MapFrom(src => src.Identity))
                .ForCtorParam("StartDate", opt => opt.MapFrom(src => src.Period.StartDate))
                .ForCtorParam("EndDate", opt => opt.MapFrom(src => src.Period.EndDate));
            CreateMap<Session, SessionItem>()
                .ForCtorParam("SessionCode", opt => opt.MapFrom(src => src.Identity))
                .ForCtorParam("Speaker", opt => opt.MapFrom(src => $"{src.Speaker.Name} {src.Speaker.Surname}"));
            CreateMap<Speaker, SpeakerDetail>()
                .ForCtorParam("SpeakerCode", opt => opt.MapFrom(src => src.Identity));
            CreateMap<Speaker, SpeakerItem>()
                .ForCtorParam("SpeakerCode", opt => opt.MapFrom(src => src.Identity));
        }
    }
}
