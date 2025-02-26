namespace ECO.Data;

public abstract class PersistenceManagerBase<T, K>(IDataContext dataContext) : IPersistenceManager<T, K>
    where T : class, IAggregateRoot<K>
{
    #region Protected_Fields
    protected readonly IDataContext _DataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
    protected IPersistenceContext? _PersistenceContext;
    #endregion

    #region Properties
    public IPersistenceContext PersistenceContext => (_PersistenceContext ??= _DataContext.GetCurrentContext<T>());
    #endregion
  
}
