namespace ECO.Providers.MongoDB.Configuration;

using System;
using System.Collections.Generic;
using ECO.Configuration;
using ECO.Data;

/// <summary>
/// Extension methods for configuring MongoDB persistence units in ECO data context options.
/// </summary>
public static class MongoDBDataContextOptionsExtensions
{
    #region Extensions
    
    /// <summary>
    /// Configures the data context to use MongoDB with fluent configuration.
    /// </summary>
    /// <param name="dataContextOptions">The data context options to configure.</param>
    /// <param name="persistenceUnitName">The name of the persistence unit.</param>
    /// <param name="optionsAction">Action to configure MongoDB specific options.</param>
    /// <returns>The configured data context options for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when any required parameter is null.</exception>
    public static DataContextOptions UseMongoDB(
        this DataContextOptions dataContextOptions, 
        string persistenceUnitName, 
        Action<MongoDBOptions> optionsAction)
    {
        ArgumentNullException.ThrowIfNull(dataContextOptions);
        ArgumentNullException.ThrowIfNull(persistenceUnitName);
        ArgumentNullException.ThrowIfNull(optionsAction);
        
        dataContextOptions.PersistenceUnitFactoryOptions += (persistenceUnitFactory, loggerFactory) =>
        {
            var options = new MongoDBOptions();
            optionsAction(options);
            
            var persistenceUnit = new MongoPersistenceUnit(persistenceUnitName, loggerFactory);
            
            // Configure with the provided options
            persistenceUnit.ConfigureWith(options);
            
            persistenceUnitFactory.AddPersistenceUnit(persistenceUnit);
        };
        
        return dataContextOptions;
    }
    
    #endregion
}

/// <summary>
/// Configuration options for MongoDB persistence units.
/// </summary>
public sealed class MongoDBOptions
{
    #region Fields
    
    /// <summary>
    /// Collection of persistence unit listeners to attach.
    /// </summary>
    private readonly List<IPersistenceUnitListener> _listeners = [];
    
    /// <summary>
    /// Collection of aggregate type assemblies to discover types from.
    /// </summary>
    private readonly List<Type> _assemblyMarkerTypes = [];
    
    /// <summary>
    /// Collection of explicit aggregate types to add.
    /// </summary>
    private readonly List<Type> _aggregateTypes = [];
    
    #endregion

    #region Properties
    
    /// <summary>
    /// Gets the collection of configured persistence unit listeners.
    /// </summary>
    public IEnumerable<IPersistenceUnitListener> Listeners => _listeners.AsReadOnly();
    
    /// <summary>
    /// Gets the collection of assembly marker types for auto-discovery.
    /// </summary>
    public IEnumerable<Type> AssemblyMarkerTypes => _assemblyMarkerTypes.AsReadOnly();
    
    /// <summary>
    /// Gets the collection of explicit aggregate types.
    /// </summary>
    public IEnumerable<Type> AggregateTypes => _aggregateTypes.AsReadOnly();
    
    /// <summary>
    /// Gets or sets the MongoDB connection string.
    /// </summary>
    public string? ConnectionString { get; set; }
    
    /// <summary>
    /// Gets or sets the MongoDB database name.
    /// </summary>
    public string? DatabaseName { get; set; }
    
    /// <summary>
    /// Gets or sets the mapping assemblies string (semicolon-separated assembly names).
    /// </summary>
    public string? MappingAssemblies { get; set; }
    
    /// <summary>
    /// Gets or sets whether to use identity map for serializers.
    /// </summary>
    public bool UseIdentityMap { get; set; } = false;
    
    #endregion

    #region Methods
    
    /// <summary>
    /// Adds a persistence unit listener of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of listener to add. Must implement IPersistenceUnitListener and have a parameterless constructor.</typeparam>
    /// <returns>This MongoDBOptions instance for method chaining.</returns>
    public MongoDBOptions AddListener<T>() where T : IPersistenceUnitListener, new()
    {
        _listeners.Add(new T());
        return this;
    }
    
    /// <summary>
    /// Adds the specified persistence unit listener instance.
    /// </summary>
    /// <param name="listener">The listener instance to add.</param>
    /// <returns>This MongoDBOptions instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when listener is null.</exception>
    public MongoDBOptions AddListener(IPersistenceUnitListener listener)
    {
        ArgumentNullException.ThrowIfNull(listener);
        _listeners.Add(listener);
        return this;
    }
    
    /// <summary>
    /// Adds an assembly marker type for auto-discovery of aggregate types.
    /// All types implementing IAggregateRoot in the assembly containing the marker type will be automatically discovered.
    /// </summary>
    /// <typeparam name="T">The marker type representing the assembly to scan.</typeparam>
    /// <returns>This MongoDBOptions instance for method chaining.</returns>
    public MongoDBOptions AddAssemblyFromType<T>()
    {
        _assemblyMarkerTypes.Add(typeof(T));
        return this;
    }
    
    /// <summary>
    /// Adds an assembly marker type for auto-discovery of aggregate types.
    /// All types implementing IAggregateRoot in the assembly containing the marker type will be automatically discovered.
    /// </summary>
    /// <param name="markerType">The marker type representing the assembly to scan.</param>
    /// <returns>This MongoDBOptions instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when markerType is null.</exception>
    public MongoDBOptions AddAssemblyFromType(Type markerType)
    {
        ArgumentNullException.ThrowIfNull(markerType);
        _assemblyMarkerTypes.Add(markerType);
        return this;
    }
    
    /// <summary>
    /// Adds a specific aggregate type to the persistence unit.
    /// </summary>
    /// <typeparam name="T">The aggregate type to add.</typeparam>
    /// <returns>This MongoDBOptions instance for method chaining.</returns>
    public MongoDBOptions AddAggregateType<T>()
    {
        _aggregateTypes.Add(typeof(T));
        return this;
    }
    
    /// <summary>
    /// Adds a specific aggregate type to the persistence unit.
    /// </summary>
    /// <param name="aggregateType">The aggregate type to add.</param>
    /// <returns>This MongoDBOptions instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when aggregateType is null.</exception>
    public MongoDBOptions AddAggregateType(Type aggregateType)
    {
        ArgumentNullException.ThrowIfNull(aggregateType);
        _aggregateTypes.Add(aggregateType);
        return this;
    }
    
    #endregion
}