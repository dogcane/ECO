using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ECO.Data;

/// <summary>
/// Base class for persistence units, providing common functionality for managing classes, listeners, and context creation.
/// </summary>
/// <typeparam name="P">The derived persistence unit type.</typeparam>
public abstract class PersistenceUnitBase<P>(string name, ILoggerFactory? loggerFactory = null) : IPersistenceUnit
    where P : PersistenceUnitBase<P>
{
    #region Protected_Fields

    /// <summary>
    /// The name of the persistence unit.
    /// </summary>
    protected string _Name = name ?? throw new ArgumentNullException(nameof(name));

    /// <summary>
    /// The set of registered aggregate root types.
    /// </summary>
    protected ISet<Type> _Classes = new HashSet<Type>();

    /// <summary>
    /// The set of registered persistence unit listeners.
    /// </summary>
    protected ISet<IPersistenceUnitListener> _Listeners = new HashSet<IPersistenceUnitListener>();

    /// <summary>
    /// The logger factory for creating loggers.
    /// </summary>
    protected readonly ILoggerFactory? _LoggerFactory = loggerFactory;

    /// <summary>
    /// The logger for this persistence unit.
    /// </summary>
    protected readonly ILogger<P>? _Logger = loggerFactory?.CreateLogger<P>();

    #endregion

    #region Public_Properties

    /// <inheritdoc/>
    public virtual string Name => _Name;

    /// <inheritdoc/>
    public virtual IEnumerable<Type> Classes => _Classes;

    /// <inheritdoc/>
    public virtual IEnumerable<IPersistenceUnitListener> Listeners => _Listeners;

    #endregion

    #region Protected_Methods

    /// <summary>
    /// Creates a new persistence context.
    /// </summary>
    /// <returns>The created persistence context.</returns>
    protected abstract IPersistenceContext OnCreateContext();

    /// <summary>
    /// Called during initialization.
    /// </summary>
    /// <param name="extendedAttributes">Extended attributes for initialization.</param>
    /// <param name="configuration">The configuration instance.</param>
    protected virtual void OnInitialize(IDictionary<string, string> extendedAttributes, IConfiguration configuration) { }

    /// <summary>
    /// Called before a context is created.
    /// </summary>
    protected virtual void OnContextPreCreate()
    {
        foreach (var listener in _Listeners)
        {
            listener.ContextPreCreate(this);
        }
    }

    /// <summary>
    /// Called after a context is created.
    /// </summary>
    /// <param name="context">The created context.</param>
    protected virtual void OnContextPostCreate(IPersistenceContext context)
    {
        foreach (var listener in _Listeners)
        {
            listener.ContextPostCreate(this, context);
        }
    }

    /// <summary>
    /// Called when a class is added.
    /// </summary>
    /// <param name="classType">The added class type.</param>
    protected virtual void OnClassAdded(Type classType) { }

    /// <summary>
    /// Called when a class is removed.
    /// </summary>
    /// <param name="classType">The removed class type.</param>
    protected virtual void OnClassRemoved(Type classType) { }

    /// <summary>
    /// Called when a unit listener is added.
    /// </summary>
    /// <param name="listener">The added listener.</param>
    protected virtual void OnUnitListenerAdded(IPersistenceUnitListener listener) { }

    /// <summary>
    /// Called when a unit listener is removed.
    /// </summary>
    /// <param name="listener">The removed listener.</param>
    protected virtual void OnUnitListenerRemoved(IPersistenceUnitListener listener) { }

    #endregion

    #region Public_Methods

    /// <inheritdoc/>
    public virtual void Initialize(IDictionary<string, string> extendedAttributes, IConfiguration configuration)
        => OnInitialize(extendedAttributes, configuration);

    /// <inheritdoc/>
    public virtual IPersistenceContext CreateContext()
    {
        OnContextPreCreate();
        var context = OnCreateContext();
        OnContextPostCreate(context);
        return context;
    }

    /// <inheritdoc/>
    public virtual IPersistenceUnit AddClass(Type classType)
    {
        ArgumentNullException.ThrowIfNull(classType);
        if (_Classes.Add(classType))
        {
            OnClassAdded(classType);
        }
        return this;
    }

    /// <inheritdoc/>
    public virtual IPersistenceUnit AddClass<T, K>() where T : class, IAggregateRoot<K>
        => AddClass(typeof(T));

    /// <inheritdoc/>
    public virtual IPersistenceUnit RemoveClass(Type classType)
    {
        ArgumentNullException.ThrowIfNull(classType);
        if (_Classes.Remove(classType))
        {
            OnClassRemoved(classType);
        }
        return this;
    }

    /// <inheritdoc/>
    public virtual IPersistenceUnit RemoveClass<T, K>() where T : class, IAggregateRoot<K>
        => RemoveClass(typeof(T));

    /// <inheritdoc/>
    public virtual IPersistenceUnit AddUnitListener(IPersistenceUnitListener listener)
    {
        ArgumentNullException.ThrowIfNull(listener);
        if (_Listeners.Add(listener))
        {
            OnUnitListenerAdded(listener);
        }
        return this;
    }

    /// <inheritdoc/>
    public virtual IPersistenceUnit RemoveUnitListener(IPersistenceUnitListener listener)
    {
        ArgumentNullException.ThrowIfNull(listener);
        if (_Listeners.Remove(listener))
        {
            OnUnitListenerRemoved(listener);
        }
        return this;
    }

    /// <inheritdoc/>
    public abstract IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext)
        where T : class, IAggregateRoot<K>
        where K : notnull, IEquatable<K>;

    /// <inheritdoc/>
    public abstract IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext)
        where T : class, IAggregateRoot<K>
        where K : notnull, IEquatable<K>;

    #endregion
}
