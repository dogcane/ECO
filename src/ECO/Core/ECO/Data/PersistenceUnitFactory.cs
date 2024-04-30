using Microsoft.Extensions.Logging;

namespace ECO.Data;

public sealed class PersistenceUnitFactory : IPersistenceUnitFactory
{
    #region Private_Fields

    private readonly Dictionary<string, IPersistenceUnit> _Units = [];

    private readonly Dictionary<Type, IPersistenceUnit> _Classes = [];

    private readonly ILoggerFactory? _LoggerFactory;

    private readonly ILogger<PersistenceUnitFactory>? _Logger;

    #endregion

    #region ~Ctor

    public PersistenceUnitFactory(ILoggerFactory? loggerFactory = null)
    {
        _LoggerFactory = loggerFactory;
        _Logger = _LoggerFactory?.CreateLogger<PersistenceUnitFactory>();
    }

    #endregion

    #region Public_Methods

    public IPersistenceUnitFactory AddPersistenceUnit(IPersistenceUnit persistenceUnit)
    {
        ArgumentNullException.ThrowIfNull(persistenceUnit);
        _Logger?.LogDebug($"Adding persistence unit {persistenceUnit.Name} of type {persistenceUnit.GetType().Name}");
        _Units.Add(persistenceUnit.Name, persistenceUnit);
        foreach (var classType in persistenceUnit.Classes)
        {
            _Classes.Add(classType, persistenceUnit);
        }
        return this;
    }

    public IPersistenceUnit GetPersistenceUnit(string name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
        if (_Units.TryGetValue(name, out IPersistenceUnit? value))
        {
            return value;
        }
        else
        {
            throw new PersistenceUnitNotFoundException(name);
        }
    }

    public IPersistenceUnit GetPersistenceUnit<T>()
    {
        return GetPersistenceUnit(typeof(T));
    }

    public IPersistenceUnit GetPersistenceUnit(Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);
        if (_Classes.TryGetValue(entityType, out IPersistenceUnit? value))
        {
            return value;
        }
        else
        {
            foreach (Type registeredType in _Classes.Keys)
            {
                if (entityType.IsSubclassOf(registeredType))
                {
                    IPersistenceUnit persistenceUnit = GetPersistenceUnit(registeredType);
                    _Logger?.LogDebug($"Mapping subclass {entityType.Name} of {registeredType.Name} in persistence unit {persistenceUnit.Name}");
                    _Classes.Add(entityType, persistenceUnit);
                    return persistenceUnit;
                }
            }
        }
        throw new PersistentClassNotRegisteredException(entityType);
    }

    public IDataContext OpenDataContext() => new DataContext(this, _LoggerFactory?.CreateLogger<DataContext>());

    #endregion
}
