using ECO.Data;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ECO.Providers.InMemory
{
    public abstract class InMemoryPersistenceManager<T, K> : PersistenceManagerBase<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Fields - Memory Storage

        protected static object _SycnLock = new object();

        protected static IDictionary<K, T> _IdentityMap = new Dictionary<K, T>();

        protected static IList<T> _EntitySet = new List<T>();

        #endregion

        #region Properties

        public virtual InMemoryPersistenceContext InMemoryPersistenceContext => PersistenceContext as InMemoryPersistenceContext;

        #endregion

        #region Ctor

        protected InMemoryPersistenceManager(IDataContext dataContext)
            : base(dataContext)
        {

        }

        #endregion
    }
}
