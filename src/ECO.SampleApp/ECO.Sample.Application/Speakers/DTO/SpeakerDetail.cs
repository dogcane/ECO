using System;

namespace ECO.Sample.Application.Speakers.DTO
{
    public class SpeakerDetail
    {
        public Guid SpeakerCode { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int Age { get; set; }

        public string Description { get; set; }
    }
}
