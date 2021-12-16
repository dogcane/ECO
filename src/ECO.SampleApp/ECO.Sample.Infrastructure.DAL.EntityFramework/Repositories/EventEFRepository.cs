using ECO.Data;
using ECO.Providers.EntityFramework;
using ECO.Sample.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ECO.Sample.Infrastructure.Repositories
{
    public class EventEFRepository : EntityFrameworkRepository<Event, Guid>, IEventRepository
    {
        public EventEFRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        public override void Update(Event item)
        {    
            CheckSessionEntry(item.Sessions);
            base.Update(item);
        }

        public override async Task UpdateAsync(Event item)
        {
            CheckSessionEntry(item.Sessions);
            await base.UpdateAsync(item);
        }

        private void CheckSessionEntry(IEnumerable<Session> sessions)
        {
            foreach (var session in sessions)
            {
                var sessionEntry = DbContext.Entry(session);
                if (sessionEntry?.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                {
                    sessionEntry.State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
            }
        }
    }
}
