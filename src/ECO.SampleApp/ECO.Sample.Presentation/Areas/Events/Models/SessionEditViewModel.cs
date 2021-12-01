﻿using System;

namespace ECO.Sample.Presentation.Areas.Events.Models
{
    public class SessionEditViewModel
    {
        public Guid EventCode { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Level { get; set; }

        public Guid SpeakerCode { get; set; }
    }
}
