using ECO.Sample.Application.Events.DTO;
using System;
using System.Linq;

namespace ECO.Sample.Application.Events
{
    public interface IShowEventsService
    {
        IQueryable<EventListItem> ShowEvents(DateTime? fromDate, DateTime? toDate, string eventName);
    }
}
