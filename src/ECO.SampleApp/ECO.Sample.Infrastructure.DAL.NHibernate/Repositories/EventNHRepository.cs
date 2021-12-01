using ECO.Data;
using ECO.Providers.NHibernate;
using ECO.Sample.Domain;
using System;

namespace ECO.Sample.Infrastructure.Repositories
{
    public class EventNHRepository : NHRepository<Event, Guid>, IEventRepository
    {
        public EventNHRepository(IDataContext dataContext) : base(dataContext)
        {
        }
    }
}
