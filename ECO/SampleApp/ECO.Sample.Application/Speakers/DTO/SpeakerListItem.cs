using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Sample.Domain;

namespace ECO.Sample.Application.Speakers.DTO
{
    public class SpeakerListItem
    {
        public Guid SpeakerCode { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public static SpeakerListItem From(Speaker speaker)
        {
            return new SpeakerListItem()
            {
                SpeakerCode = speaker.Identity,
                Name = speaker.Name,
                Surname  = speaker.Surname
            };
        }
    }
}
