using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Sample.Domain;

namespace ECO.Sample.Application.Speakers.DTO
{
    public class SpeakerDetail
    {
        public Guid SpeakerCode { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int Age { get; set; }

        public string Description { get; set; }

        public static SpeakerDetail From(Speaker speaker)
        {
            return new SpeakerDetail()
            {
                SpeakerCode = speaker.Identity,
                Name = speaker.Name,
                Surname = speaker.Surname,
                Age = speaker.Age,
                Description = speaker.Description
            };
        }
    }
}
