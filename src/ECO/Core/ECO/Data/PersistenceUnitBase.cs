using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ECO.Data;

public abstract class PersistenceUnitBase<P> : IPersistenceUnit
    where P : PersistenceUnitBase<P>
{
    #region Protected_Fields

    protected string _Name;

    protected ISet<Type> _Classes = new HashSet<Type>();

    protected ISet<IPersistenceUnitListener> _Listeners = new HashSet<IPersistenceUnitListener>();

    protected readonly ILoggerFactory? _LoggerFactory;

    protected readonly ILogger<P>? _Logger;

    #endregion

    #region Public_Properties

    public virtual string Name => _Name;

    public virtual IEnumerable<Type> Classes => _Classes;

    public virtual IEnumerable<IPersistenceUnitListener> Listeners => _Listeners;

    #endregion

    #region Ctor

    protected PersistenceUnitBase(string name, ILoggerFactory? loggerFactory = null)
    {
        _Name = name ?? throw new ArgumentNullException(nameof(name));
        _LoggerFactory = loggerFactory;
        _Logger = loggerFactory?.CreateLogger<P>();
    }

    #endregion

    #region Protected_Methods

    protected abstract IPersistenceContext OnCreateContext();

    protected virtual void OnInitialize(IDictionary<string, string> extendedAttributes, IConfiguration configuration)
    {

    }

    protected virtual void OnContextPreCreate()
    {
        foreach (IPersistenceUnitListener listener in _Listeners)
        {
            listener.ContextPreCreate(this);
        }
    }

    protected virtual void OnContextPostCreate(IPersistenceContext context)
    {
        foreach (IPersistenceUnitListener listener in _Listeners)
        {
            listener.ContextPostCreate(this, context);
        }
    }

    protected virtual void OnClassAdded(Type classType)
    {

    }

    protected virtual void OnClassRemoved(Type classType)
    {

    }

    protected virtual void OnUnitListenerAdded(IPersistenceUnitListener listener)
    {

    }

    protected virtual void OnUnitListenerRemoved(IPersistenceUnitListener listener)
    {

    }

    #endregion

    #region Public_Methods
    public virtual void Initialize(IDictionary<string, string> extededAttributes, IConfiguration configuration)
    {
        OnInitialize(extededAttributes, configuration);
    }

    public virtual IPersistenceContext CreateContext()
    {
        OnContextPreCreate();
        IPersistenceContext context = OnCreateContext();
        OnContextPostCreate(context);
        return context;
    }

    public virtual IPersistenceUnit AddClass(Type classType)
    {
        ArgumentNullException.ThrowIfNull(classType);
        if (_Classes.Contains(classType)) return this;

        _Classes.Add(classType);
        OnClassAdded(classType);
        return this;
    }

    public virtual IPersistenceUnit AddClass<T, K>() where T : class, IAggregateRoot<K>
    {
        return AddClass(typeof(T));
    }

    public virtual IPersistenceUnit RemoveClass(Type classType)
    {
        ArgumentNullException.ThrowIfNull(classType);
        if (!_Classes.Contains(classType)) return this;

        _Classes.Remove(classType);
        OnClassRemoved(classType);
        return this;
    }

    public virtual IPersistenceUnit RemoveClass<T, K>() where T : class, IAggregateRoot<K>
    {
        return RemoveClass(typeof(T));
    }

    public virtual IPersistenceUnit AddUnitListener(IPersistenceUnitListener listener)
    {
        ArgumentNullException.ThrowIfNull(listener);
        if (_Listeners.Contains(listener)) return this;

        _Listeners.Add(listener);
        OnUnitListenerAdded(listener);
        return this;
    }

    public virtual IPersistenceUnit RemoveUnitListener(IPersistenceUnitListener listener)
    {
        ArgumentNullException.ThrowIfNull(listener);
        if (!_Listeners.Contains(listener)) return this;

        _Listeners.Remove(listener);
        OnUnitListenerRemoved(listener);
        return this;
    }

    public abstract IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext) where T : class, IAggregateRoot<K>;

    public abstract IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext) where T : class, IAggregateRoot<K>;

    #endregion
}
