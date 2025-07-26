namespace ECO.Data;

/// <summary>
/// Provides a base implementation for persistence managers, coordinating access to the persistence context for a specific aggregate root type.
/// </summary>
/// <typeparam name="T">The aggregate root type managed by this persistence manager.</typeparam>
/// <typeparam name="K">The type of the aggregate root's identity.</typeparam>
public abstract class PersistenceManagerBase<T, K>(IDataContext dataContext) : IPersistenceManager<T, K>
    where T : class, IAggregateRoot<K>
    where K : notnull, IEquatable<K>
{
    #region Protected_Fields
    /// <summary>
    /// The data context used to obtain persistence contexts.
    /// </summary>
    protected readonly IDataContext _DataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));

    /// <summary>
    /// The cached persistence context for the current aggregate root type.
    /// </summary>
    protected IPersistenceContext? _PersistenceContext;
    #endregion

    #region Properties
    /// <summary>
    /// Gets the current persistence context for the aggregate root type <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// The context is lazily initialized and cached for subsequent accesses.
    /// </remarks>
    public IPersistenceContext PersistenceContext => _PersistenceContext ??= _DataContext.GetCurrentContext<T>();
    #endregion
}
