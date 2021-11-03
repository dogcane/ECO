using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Providers.MongoDB;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

using ECO.Sample.Domain;

namespace ECO.Sample.Infrastructure.Repositories
{
    public class EventMongoRepository : MongoRepository<Event, Guid>, IEventRepository
    {

    }
}
