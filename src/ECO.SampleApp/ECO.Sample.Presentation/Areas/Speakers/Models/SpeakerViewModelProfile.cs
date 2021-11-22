using AutoMapper;
using ECO.Sample.Application.Speakers.DTO;

namespace ECO.Sample.Presentation.Areas.Speakers.Models
{
    public class SpeakerViewModelProfile : Profile
    {
        public SpeakerViewModelProfile()
        {
            CreateMap<SpeakerItem, SpeakerItemViewModel>();
            CreateMap<SpeakerDetail, SpeakerViewModel>();
        }

    }
}
