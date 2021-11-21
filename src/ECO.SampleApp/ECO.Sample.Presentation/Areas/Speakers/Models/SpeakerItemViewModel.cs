using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Sample.Domain;

namespace ECO.Sample.Presentation.Areas.Speakers.Models
{
    public class SpeakerItemViewModel
    {
        public Guid SpeakerCode { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
