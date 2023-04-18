using ECO.Data;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Providers.InMemory
{
    public class InMemoryPersistenceContext : PersistenceContextBase<InMemoryPersistenceContext>
    {
        #region Ctor

        public InMemoryPersistenceContext(IPersistenceUnit persistenceUnit, ILogger<InMemoryPersistenceContext>? logger)
            : base(persistenceUnit, logger)
        {

        }

        #endregion
    }
}
