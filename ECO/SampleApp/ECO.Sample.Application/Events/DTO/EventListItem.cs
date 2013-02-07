using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Sample.Domain;

namespace ECO.Sample.Application.Events.DTO
{
    public class EventListItem
    {
        public Guid EventCode { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int NumberOfSessions { get; set; }

        public static EventListItem From(Event @event)
        {
            return new EventListItem()
            {
                EventCode = @event.Identity,
                Name = @event.Name,
                StartDate = @event.Period.StartDate,
                EndDate = @event.Period.EndDate,
                NumberOfSessions = @event.Sessions.Count()
            };
        }
    }
}
