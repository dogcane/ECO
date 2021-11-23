using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Data;
using ECO.Providers.EntityFramework;

using ECO.Sample.Domain;

namespace ECO.Sample.Infrastructure.Repositories
{
    public class EventEFRepository : EntityFrameworkRepository<Event, Guid>, IEventRepository
    {
        public EventEFRepository(IDataContext dataContext) : base(dataContext)
        {
        }
    }
}
