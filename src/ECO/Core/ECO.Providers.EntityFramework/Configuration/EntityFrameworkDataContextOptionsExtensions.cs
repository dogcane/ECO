using ECO.Configuration;
using ECO.Data;
using ECO.Providers.EntityFramework.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ECO.Providers.EntityFramework.Configuration;

public static class EntityFrameworkDataContextOptionsExtensions
{
    public static DataContextOptions UseEntityFramework<T>(this DataContextOptions dataContextOptions, string persistenceUnitName, Action<EntityFrameworkOptions> optionsAction)
        where T : DbContext
    {
        ArgumentNullException.ThrowIfNull(dataContextOptions);
        ArgumentNullException.ThrowIfNull(persistenceUnitName);
        ArgumentNullException.ThrowIfNull(optionsAction);
        dataContextOptions.PersistenceUnitFactoryOptions += (persistenceUnitFactory, loggerFactory) =>
        {
            EntityFrameworkOptions options = new();
            optionsAction(options);
            EntityFrameworkPersistenceUnit<T> persistenceUnit = new(persistenceUnitName, options.DbContextOptions.Options, loggerFactory);
            foreach (var entityType in DbContextFacade<T>.GetAggregateTypesFromDBContext())
            {
                persistenceUnit.AddClass(entityType);
            }
            foreach (var listener in options.Listeners)
            {
                persistenceUnit.AddUnitListener(listener);
            }
            persistenceUnitFactory.AddPersistenceUnit(persistenceUnit);
        };
        return dataContextOptions;
    }
}

public sealed class EntityFrameworkOptions
{
    #region Fields
    private readonly List<IPersistenceUnitListener> listeners = [];
    #endregion

    #region Properties
    public IEnumerable<IPersistenceUnitListener> Listeners => listeners;
    public DbContextOptionsBuilder DbContextOptions { get; private set; } = new DbContextOptionsBuilder();
    #endregion

    #region Methods
    public EntityFrameworkOptions AddListener<T>()
        where T : IPersistenceUnitListener, new()
    {
        listeners.Add(new T());
        return this;
    }
    public EntityFrameworkOptions AddListener(IPersistenceUnitListener listener)
    {
        ArgumentNullException.ThrowIfNull(listener);
        listeners.Add(listener);
        return this;
    }
    #endregion
}
