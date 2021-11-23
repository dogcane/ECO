using ECO.Data;
using Microsoft.Extensions.Logging;

namespace ECO.Providers.InMemory
{
    public sealed class InMemoryPersistenceUnit : PersistenceUnitBase<InMemoryPersistenceUnit>
    {
        #region Ctor

        public InMemoryPersistenceUnit(ILoggerFactory loggerFactory)
            :base(loggerFactory)
        {

        }

        #endregion

        #region Methods

        protected override IPersistenceContext OnCreateContext() => new InMemoryPersistenceContext(this, _LoggerFactory);

        public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext) => new InMemoryReadOnlyRepository<T, K>(dataContext);

        public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext) => new InMemoryRepository<T, K>(dataContext);

        #endregion        
    }
}
