using AutoMapper;
using ECO.Sample.Application.Events.DTO;

namespace ECO.Sample.Presentation.Areas.Events.Models
{
    public class EventViewModelProfile : Profile
    {
        public EventViewModelProfile()
        {
            CreateMap<EventItem, EventItemViewModel>();
            CreateMap<EventDetail, EventViewModel>();
            CreateMap<SessionItem, SessionItemViewModel>();
        }

    }
}
