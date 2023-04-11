using ECO.Data;
using ECO.Providers.Marten;
using ECO.Sample.Domain;
using System;

namespace ECO.Sample.Infrastructure.Repositories
{
    public class EventMartenRepository : MartenRepository<Event, Guid>, IEventRepository
    {
        public EventMartenRepository(IDataContext dataContext)
            : base(dataContext)
        {

        }
    }
}
