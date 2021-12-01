using ECO.Data;
using Microsoft.Extensions.Logging;

namespace ECO.Providers.InMemory
{
    public class InMemoryPersistenceContext : PersistenceContextBase<InMemoryPersistenceContext>
    {
        #region Ctor

        public InMemoryPersistenceContext(IPersistenceUnit persistenceUnit, ILogger<InMemoryPersistenceContext> logger)
            : base(persistenceUnit, logger)
        {

        }

        #endregion

        #region Methods

        protected override IDataTransaction OnBeginTransaction() => new NullDataTransaction(this);

        #endregion
    }
}
