using ECO.Data;
using ECO.Providers.EntityFramework;
using ECO.Sample.Domain;
using System;

namespace ECO.Sample.Infrastructure.Repositories
{
    public class EventEFRepository : EntityFrameworkRepository<Event, Guid>, IEventRepository
    {
        public EventEFRepository(IDataContext dataContext) : base(dataContext)
        {
        }
    }
}
