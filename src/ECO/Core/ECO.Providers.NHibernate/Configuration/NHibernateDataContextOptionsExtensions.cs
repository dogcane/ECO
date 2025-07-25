namespace ECO.Providers.NHibernate.Configuration;

using System;
using System.Collections.Generic;
using ECO.Configuration;
using ECO.Data;
using NhCfg = global::NHibernate.Cfg;

/// <summary>
/// Extension methods for configuring NHibernate persistence units in ECO data context options.
/// </summary>
public static class NHibernateDataContextOptionsExtensions
{
    #region Extensions
    
    /// <summary>
    /// Configures the data context to use NHibernate with FluentNHibernate configuration.
    /// </summary>
    /// <param name="dataContextOptions">The data context options to configure.</param>
    /// <param name="persistenceUnitName">The name of the persistence unit.</param>
    /// <param name="optionsAction">Action to configure NHibernate specific options.</param>
    /// <returns>The configured data context options for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when any required parameter is null.</exception>
    public static DataContextOptions UseNHibernate(
        this DataContextOptions dataContextOptions, 
        string persistenceUnitName, 
        Action<NHibernateOptions> optionsAction)
    {
        ArgumentNullException.ThrowIfNull(dataContextOptions);
        ArgumentNullException.ThrowIfNull(persistenceUnitName);
        ArgumentNullException.ThrowIfNull(optionsAction);
        
        dataContextOptions.PersistenceUnitFactoryOptions += (persistenceUnitFactory, loggerFactory) =>
        {
            var options = new NHibernateOptions();
            optionsAction(options);
            
            var persistenceUnit = new NHPersistenceUnit(persistenceUnitName, loggerFactory);
            
            // Configure with the provided options
            persistenceUnit.ConfigureWith(options);
            
            persistenceUnitFactory.AddPersistenceUnit(persistenceUnit);
        };
        
        return dataContextOptions;
    }
    
    #endregion
}

/// <summary>
/// Configuration options for NHibernate persistence units.
/// </summary>
public sealed class NHibernateOptions
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
    /// Gets or sets the database configuration for FluentNHibernate.
    /// </summary>
    public NhCfg.Configuration Configuration { get; init; } = new ();
    
    #endregion

    #region Methods
    
    /// <summary>
    /// Adds a persistence unit listener of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of listener to add. Must implement IPersistenceUnitListener and have a parameterless constructor.</typeparam>
    /// <returns>This NHibernateOptions instance for method chaining.</returns>
    public NHibernateOptions AddListener<T>() where T : IPersistenceUnitListener, new()
    {
        _listeners.Add(new T());
        return this;
    }
    
    /// <summary>
    /// Adds the specified persistence unit listener instance.
    /// </summary>
    /// <param name="listener">The listener instance to add.</param>
    /// <returns>This NHibernateOptions instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when listener is null.</exception>
    public NHibernateOptions AddListener(IPersistenceUnitListener listener)
    {
        ArgumentNullException.ThrowIfNull(listener);
        _listeners.Add(listener);
        return this;
    }
    
    #endregion
}
