using System;
using System.Collections.Generic;

namespace ECO.Sample.Application.Events.DTO
{
    public class EventDetail
    {
        public static readonly EventDetail Empty = new EventDetail();

        public Guid EventCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public IEnumerable<SessionListItem> Sessions { get; set; }
    }
}
