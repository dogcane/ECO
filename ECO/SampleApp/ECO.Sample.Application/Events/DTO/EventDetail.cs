using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

using ECO.Sample.Domain;

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

        public static EventDetail From(Event @event)
        {
            return new EventDetail()
            {
                EventCode = @event.Identity,
                Name = @event.Name,
                Description = @event.Description,
                StartDate = @event.Period.StartDate,
                EndDate = @event.Period.EndDate,
                Sessions = @event.Sessions.Select(item => SessionListItem.From(item))
            };
        }
    }
}
