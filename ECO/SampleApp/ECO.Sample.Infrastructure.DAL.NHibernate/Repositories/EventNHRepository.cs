using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Providers.NHibernate;

using ECO.Sample.Domain;

namespace ECO.Sample.Infrastructure.Repositories
{
    public class EventNHRepository : NHRepository<Event, Guid>, IEventRepository
    {

    }
}
