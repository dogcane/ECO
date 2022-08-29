using ECO.Data;
using ECO.Providers.MongoDB;
using ECO.Sample.Domain;
using System;

namespace ECO.Sample.Infrastructure.Repositories
{
    public class EventMongoRepository : MongoRepository<Event, Guid>, IEventRepository
    {
        public EventMongoRepository(IDataContext dataContext)
            : base(dataContext)
        {

        }

    }
}
