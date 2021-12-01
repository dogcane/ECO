using System;

namespace ECO.Sample.Presentation.Areas.Speakers.Models
{
    public class SpeakerViewModel
    {
        public static readonly SpeakerViewModel Empty = new SpeakerViewModel();

        public Guid SpeakerCode { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int Age { get; set; }

        public string Description { get; set; }
    }
}
