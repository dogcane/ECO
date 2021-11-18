using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Events.DTO
{
    public record EventDetail(Guid EventCode, string Name, string Description, DateTime StartDate, DateTime EndDate, IEnumerable<SessionItem> SessionItems);
}
