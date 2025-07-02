using ECO.Configuration;
using ECO.Data;
using ECO.Utils;
using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ECO.Providers.Marten.Configuration;

public static class MartenDataContextOptionsExtensions
{
    public static DataContextOptions UseMarten(this DataContextOptions dataContextOptions, string persistenceUnitName, Action<MartenOptions> optionsAction)
    {
        ArgumentNullException.ThrowIfNull(dataContextOptions);
        ArgumentNullException.ThrowIfNull(persistenceUnitName);
        ArgumentNullException.ThrowIfNull(optionsAction);
        dataContextOptions.PersistenceUnitFactoryOptions += (persistenceUnitFactory, loggerFactory) =>
        {
            MartenOptions options = new();
            optionsAction(options);
            options.StoreOptions.Policies.OnDocuments(new ECODocumentPolicy());
            DocumentStore store = new(options.StoreOptions);
            MartenPersistenceUnit persistenceUnit = new(persistenceUnitName, loggerFactory, store);
            foreach (var @class in options.Assemblies.SelectMany(asm => asm.ExportedTypes).OfAggregateRootType())
            {
                persistenceUnit.AddClass(@class);
            }
            foreach (var @class in options.Classes)
            {
                persistenceUnit.AddClass(@class);
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

public sealed class MartenOptions
{
    #region Fields
    private readonly List<Assembly> assemblies = [];
    private readonly List<Type> classes = [];
    private readonly List<IPersistenceUnitListener> listeners = [];
    #endregion

    #region Properties
    public IEnumerable<Assembly> Assemblies => assemblies;
    public IEnumerable<Type> Classes => classes;
    public IEnumerable<IPersistenceUnitListener> Listeners => listeners;
    public StoreOptions StoreOptions { get; private set; } = new StoreOptions();
    #endregion

    #region Methods
    public MartenOptions AddAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        assemblies.Add(assembly);
        return this;
    }
    public MartenOptions AddAssemblyFromType<T>()
    {
        return AddAssembly(typeof(T).Assembly);
    }
    public MartenOptions AddClass<T,K>()
        where T : class, IAggregateRoot<K>
    {
        return AddClass(typeof(T));
    }
    public MartenOptions AddClass(Type @class)
    {
        ArgumentNullException.ThrowIfNull(@class);
        classes.Add(@class);
        return this;
    }
    public MartenOptions AddListener<T>()
        where T : IPersistenceUnitListener, new()
    {
        listeners.Add(new T());
        return this;
    }
    public MartenOptions AddListener(IPersistenceUnitListener listener)
    {
        ArgumentNullException.ThrowIfNull(listener);
        listeners.Add(listener);
        return this;
    }

    #endregion
}
