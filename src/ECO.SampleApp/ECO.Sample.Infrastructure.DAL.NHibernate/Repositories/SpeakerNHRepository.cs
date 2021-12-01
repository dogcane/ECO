using ECO.Data;
using ECO.Providers.NHibernate;
using ECO.Sample.Domain;
using System;

namespace ECO.Sample.Infrastructure.Repositories
{
    public class SpeakerNHRepository : NHRepository<Speaker, Guid>, ISpeakerRepository
    {
        public SpeakerNHRepository(IDataContext dataContext) : base(dataContext)
        {
        }
    }
}
