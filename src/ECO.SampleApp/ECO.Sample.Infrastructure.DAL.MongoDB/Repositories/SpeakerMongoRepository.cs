using ECO.Data;
using ECO.Providers.MongoDB;
using ECO.Sample.Domain;
using System;

namespace ECO.Sample.Infrastructure.Repositories
{
    public class SpeakerMongoRepository : MongoRepository<Speaker, Guid>, ISpeakerRepository
    {
        public SpeakerMongoRepository(IDataContext dataContext)
            : base(dataContext)
        {

        }
    }
}
