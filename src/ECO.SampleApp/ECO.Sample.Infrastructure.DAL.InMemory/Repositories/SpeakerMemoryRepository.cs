using ECO.Data;
using ECO.Providers.InMemory;
using ECO.Sample.Domain;
using System;

namespace ECO.Sample.Infrastructure.Repositories
{
    public class SpeakerMemoryRepository : InMemoryRepository<Speaker, Guid>, ISpeakerRepository
    {
        public SpeakerMemoryRepository(IDataContext dataContext)
            : base(dataContext)
        {

        }

    }
}
