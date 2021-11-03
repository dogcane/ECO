using ECO.Sample.Application.Speakers.DTO;
using System.Linq;

namespace ECO.Sample.Application.Speakers
{
    public interface IShowSpeakersService
    {
        IQueryable<SpeakerListItem> ShowSpeakers(string nameOrSurname);
    }
}
