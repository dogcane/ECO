namespace ECO.Providers.EntityFramework.Configuration;

using ECO.Configuration;
using ECO.Data;
using ECO.Providers.EntityFramework.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

/// <summary>
/// Extension methods for configuring Entity Framework persistence units in ECO data context options.
/// </summary>
public static class EntityFrameworkDataContextOptionsExtensions
{
    #region Extensions
    
    /// <summary>
    /// Configures the data context to use Entity Framework with the specified DbContext type.
    /// </summary>
    /// <typeparam name="T">The DbContext type to use for Entity Framework operations.</typeparam>
    /// <param name="dataContextOptions">The data context options to configure.</param>
    /// <param name="persistenceUnitName">The name of the persistence unit.</param>
    /// <param name="optionsAction">Action to configure Entity Framework specific options.</param>
    /// <returns>The configured data context options for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when any required parameter is null.</exception>
    public static DataContextOptions UseEntityFramework<T>(
        this DataContextOptions dataContextOptions, 
        string persistenceUnitName, 
        Action<EntityFrameworkOptions> optionsAction)
        where T : DbContext
    {
        ArgumentNullException.ThrowIfNull(dataContextOptions);
        ArgumentNullException.ThrowIfNull(persistenceUnitName);
        ArgumentNullException.ThrowIfNull(optionsAction);
        
        dataContextOptions.PersistenceUnitFactoryOptions += (persistenceUnitFactory, loggerFactory) =>
        {
            var options = new EntityFrameworkOptions();
            optionsAction(options);
            
            var persistenceUnit = new EntityFrameworkPersistenceUnit<T>(
                persistenceUnitName, 
                options.DbContextOptions.Options, 
                loggerFactory);
            
            // Auto-discover aggregate types from the DbContext
            var aggregateTypes = DbContextFacade<T>.GetAggregateTypesFromDBContext();
            foreach (var entityType in aggregateTypes)
            {
                persistenceUnit.AddClass(entityType);
            }
            
            // Add configured listeners
            foreach (var listener in options.Listeners)
            {
                persistenceUnit.AddUnitListener(listener);
            }
            
            persistenceUnitFactory.AddPersistenceUnit(persistenceUnit);
        };
        
        return dataContextOptions;
    }
    
    #endregion
}

/// <summary>
/// Configuration options for Entity Framework persistence units.
/// </summary>
public sealed class EntityFrameworkOptions
{
    #region Fields
    
    /// <summary>
    /// Collection of persistence unit listeners to attach.
    /// </summary>
    private readonly List<IPersistenceUnitListener> _listeners = [];
    
    #endregion

    #region Properties
    
    /// <summary>
    /// Gets the collection of configured persistence unit listeners.
    /// </summary>
    public IEnumerable<IPersistenceUnitListener> Listeners => _listeners.AsReadOnly();
    
    /// <summary>
    /// Gets the DbContext options builder for configuring Entity Framework.
    /// </summary>
    public DbContextOptionsBuilder DbContextOptions { get; } = new();
    
    #endregion

    #region Methods
    
    /// <summary>
    /// Adds a persistence unit listener of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of listener to add. Must implement IPersistenceUnitListener and have a parameterless constructor.</typeparam>
    /// <returns>This EntityFrameworkOptions instance for method chaining.</returns>
    public EntityFrameworkOptions AddListener<T>() where T : IPersistenceUnitListener, new()
    {
        _listeners.Add(new T());
        return this;
    }
    
    /// <summary>
    /// Adds the specified persistence unit listener instance.
    /// </summary>
    /// <param name="listener">The listener instance to add.</param>
    /// <returns>This EntityFrameworkOptions instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when listener is null.</exception>
    public EntityFrameworkOptions AddListener(IPersistenceUnitListener listener)
    {
        ArgumentNullException.ThrowIfNull(listener);
        _listeners.Add(listener);
        return this;
    }
    
    #endregion
}
