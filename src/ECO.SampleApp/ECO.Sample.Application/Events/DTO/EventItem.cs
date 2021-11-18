using ECO.Sample.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Events.DTO
{
    public record EventItem(Guid EventCode, string Name, DateTime StartDate, DateTime EndDate, int NumberOfSessions);
}
