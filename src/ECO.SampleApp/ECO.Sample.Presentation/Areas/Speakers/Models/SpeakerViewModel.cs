using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

using AutoMapper;

using ECO.Sample.Domain;

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
