using ECO.Data;
using System.Collections.Concurrent;

namespace ECO.Providers.InMemory;

public abstract class InMemoryPersistenceManager<T, K> : PersistenceManagerBase<T, K>
    where T : class, IAggregateRoot<K>
    where K : notnull
{
    #region Fields - Memory Storage

    protected readonly static ConcurrentDictionary<K, T> _EntitySet = new();

    #endregion

    #region Properties

    public virtual InMemoryPersistenceContext InMemoryPersistenceContext => PersistenceContext as InMemoryPersistenceContext ?? throw new InvalidCastException(nameof(InMemoryPersistenceContext));

    #endregion

    #region Ctor

    protected InMemoryPersistenceManager(IDataContext dataContext)
        : base(dataContext)
    {

    }

    #endregion
}
