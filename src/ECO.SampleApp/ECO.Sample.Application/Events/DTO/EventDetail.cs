using System;
using System.Collections.Generic;

namespace ECO.Sample.Application.Events.DTO
{
    public record EventDetail(Guid EventCode, string Name, string Description, DateTime StartDate, DateTime EndDate, IEnumerable<SessionItem> Sessions);
}
