using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Bender;

namespace ECO.Sample.Domain
{
    public interface IEventRepository : IRepository<Event, Guid>
    {

    }
}
