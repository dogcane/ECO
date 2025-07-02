using Microsoft.Extensions.Logging;

namespace ECO.Data;

/// <summary>
/// Factory for managing and resolving persistence units by name or entity type.
/// </summary>
public sealed class PersistenceUnitFactory : IPersistenceUnitFactory
{
    #region Private_Fields

    private readonly Dictionary<string, IPersistenceUnit> _Units = [];
    private readonly Dictionary<Type, IPersistenceUnit> _Classes = [];
    private readonly ILoggerFactory? _LoggerFactory;
    private readonly ILogger<PersistenceUnitFactory>? _Logger;

    #endregion

    #region ~Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="PersistenceUnitFactory"/> class.
    /// </summary>
    /// <param name="loggerFactory">Optional logger factory for diagnostics.</param>
    public PersistenceUnitFactory(ILoggerFactory? loggerFactory = null)
    {
        _LoggerFactory = loggerFactory;
        _Logger = _LoggerFactory?.CreateLogger<PersistenceUnitFactory>();
    }

    #endregion

    #region Public_Methods

    /// <inheritdoc/>
    public IPersistenceUnitFactory AddPersistenceUnit(IPersistenceUnit persistenceUnit)
    {
        ArgumentNullException.ThrowIfNull(persistenceUnit);
        _Logger?.LogDebug("Adding persistence unit {UnitName} of type {UnitType}", persistenceUnit.Name, persistenceUnit.GetType().Name);

        if (!_Units.TryAdd(persistenceUnit.Name, persistenceUnit))
            throw new ArgumentException($"A persistence unit with the name '{persistenceUnit.Name}' is already registered.", nameof(persistenceUnit));

        foreach (var classType in persistenceUnit.Classes)
        {
            if (!_Classes.TryAdd(classType, persistenceUnit))
                _Logger?.LogWarning("Class type {ClassType} is already registered to a persistence unit.", classType.FullName);
        }
        return this;
    }

    /// <inheritdoc/>
    public IPersistenceUnit GetPersistenceUnit(string name)
    {        
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        return _Units.TryGetValue(name, out var value)
            ? value
            : throw new PersistenceUnitNotFoundException(name);
    }

    /// <inheritdoc/>
    public IPersistenceUnit GetPersistenceUnit<T>() => GetPersistenceUnit(typeof(T));

    /// <inheritdoc/>
    public IPersistenceUnit GetPersistenceUnit(Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);

        if (_Classes.TryGetValue(entityType, out var value))
            return value;

        KeyValuePair<Type, IPersistenceUnit>? match = null;
        foreach (var entry in _Classes)
        {
            if (entityType.IsSubclassOf(entry.Key))
            {
                match = entry;
                break;
            }
        }

        if (match.HasValue)
        {
            var registeredType = match.Value.Key;
            var persistenceUnit = match.Value.Value;
            _Logger?.LogDebug("Mapping subclass {EntityType} of {RegisteredType} in persistence unit {UnitName}",
                entityType.Name, registeredType.Name, persistenceUnit.Name);

            _Classes[entityType] = persistenceUnit;
            return persistenceUnit;
        }

        throw new PersistentClassNotRegisteredException(entityType);
    }

    /// <inheritdoc/>
    public IDataContext OpenDataContext()
        => new DataContext(this, _LoggerFactory?.CreateLogger<DataContext>());

    #endregion
}
