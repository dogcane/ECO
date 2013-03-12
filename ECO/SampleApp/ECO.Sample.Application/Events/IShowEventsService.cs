using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Sample.Domain;

using ECO.Sample.Application.Events.DTO;
using ECO.Sample.Application.DTO;

namespace ECO.Sample.Application.Events
{
    public interface IShowEventsService
    {
        PageableList<EventListItem> ShowEvents(DateTime? fromDate, DateTime? toDate, int page, int pageSize);
    }
}
