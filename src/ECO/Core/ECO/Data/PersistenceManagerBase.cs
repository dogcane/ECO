namespace ECO.Data;

public abstract class PersistenceManagerBase<T, K> : IPersistenceManager<T, K>
    where T : class, IAggregateRoot<K>
{
    #region Protected_Fields

    protected readonly IDataContext _DataContext;
    protected IPersistenceContext? _PersistenceContext;

    #endregion

    #region Properties
    public IPersistenceContext PersistenceContext => (_PersistenceContext ??= _DataContext.GetCurrentContext(typeof(T)));

    #endregion

    #region ~Ctor

    protected PersistenceManagerBase(IDataContext dataContext)
    {
        _DataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
    }

    #endregion
}
