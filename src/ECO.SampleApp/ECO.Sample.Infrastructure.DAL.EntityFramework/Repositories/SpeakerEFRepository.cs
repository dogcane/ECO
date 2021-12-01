using ECO.Data;
using ECO.Providers.EntityFramework;
using ECO.Sample.Domain;
using System;

namespace ECO.Sample.Infrastructure.Repositories
{
    public class SpeakerEFRepository : EntityFrameworkRepository<Speaker, Guid>, ISpeakerRepository
    {
        public SpeakerEFRepository(IDataContext dataContext) : base(dataContext)
        {
        }
    }
}
