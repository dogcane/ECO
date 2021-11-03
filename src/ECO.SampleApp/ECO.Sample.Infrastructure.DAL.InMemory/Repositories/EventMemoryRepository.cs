using ECO.Data;
using ECO.Providers.InMemory;
using ECO.Sample.Domain;
using System;

namespace ECO.Sample.Infrastructure.Repositories
{
    public class EventMemoryRepository : InMemoryRepository<Event, Guid>, IEventRepository
    {
        public EventMemoryRepository(IDataContext dataContext)
            : base(dataContext)
        {

        }
    }
}
