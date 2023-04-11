using ECO.Data;
using ECO.Providers.Marten;
using ECO.Sample.Domain;
using System;

namespace ECO.Sample.Infrastructure.Repositories
{
    public class SpeakerMartenRepository : MartenRepository<Speaker, Guid>, ISpeakerRepository
    {
        public SpeakerMartenRepository(IDataContext dataContext)
            : base(dataContext)
        {

        }

    }
}
