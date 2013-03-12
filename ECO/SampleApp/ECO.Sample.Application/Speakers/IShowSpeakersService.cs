using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Sample.Application.Speakers.DTO;

namespace ECO.Sample.Application.Speakers
{
    public interface IShowSpeakersService
    {
        IQueryable<SpeakerListItem> ShowSpeakers(string nameOrSurname);
    }
}
