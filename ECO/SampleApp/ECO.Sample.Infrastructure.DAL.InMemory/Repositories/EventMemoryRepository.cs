using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Providers.InMemory;

using ECO.Sample.Domain;

namespace ECO.Sample.Infrastructure.Repositories
{
    public class EventMemoryRepository : InMemoryRepository<Event, Guid>, IEventRepository
    {

    }
}
