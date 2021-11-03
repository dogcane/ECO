using ECO.Data;

namespace ECO.Providers.InMemory
{
    public class InMemoryPersistenceUnit : PersistenceUnitBase
    {
        #region Methods

        protected override IPersistenceContext OnCreateContext()
        {
            return new InMemoryPersistenceContext();
        }

        public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext)
        {
            return new InMemoryReadOnlyRepository<T, K>(dataContext);
        }

        public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext)
        {
            return new InMemoryRepository<T, K>(dataContext);
        }

        #endregion        
    }
}
