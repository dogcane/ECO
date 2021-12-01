using System;

namespace ECO.Sample.Application.Events.DTO
{
    public record EventItem(Guid EventCode, string Name, DateTime StartDate, DateTime EndDate, int NumberOfSessions);
}
