﻿using System;

namespace ECO.Sample.Application.Events.DTO
{
    public class EventListItem
    {
        public Guid EventCode { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int NumberOfSessions { get; set; }
    }
}
