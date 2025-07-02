using ECO.Configuration;
using ECO.Data;
using ECO.Utils;
using System.Reflection;

namespace ECO.Providers.InMemory.Configuration;

public static class InMemoryDataContextOptionsExtensions
{
    public static DataContextOptions UseInMemory(this DataContextOptions dataContextOptions, string persistenceUnitName, Action<InMemoryOptions> optionsAction)
    {            
        ArgumentNullException.ThrowIfNull(dataContextOptions);
        ArgumentNullException.ThrowIfNull(persistenceUnitName);
        ArgumentNullException.ThrowIfNull(optionsAction);
        dataContextOptions.PersistenceUnitFactoryOptions += (persistenceUnitFactory, loggerFactory) =>
        {
            InMemoryOptions options = new() { };
            optionsAction(options);
            InMemoryPersistenceUnit persistenceUnit = new(persistenceUnitName, loggerFactory);
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

public sealed class InMemoryOptions
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
    #endregion

    #region Methods
    public InMemoryOptions AddAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        assemblies.Add(assembly);
        return this;
    }

    public InMemoryOptions AddAssemblyFromType<T>() => AddAssembly(typeof(T).Assembly);

    public InMemoryOptions AddClass<T, K>()
        where T : class, IAggregateRoot<K> => AddClass(typeof(T));

    public InMemoryOptions AddClass(Type @class)
    {
        ArgumentNullException.ThrowIfNull(@class);
        classes.Add(@class);
        return this;
    }
    public InMemoryOptions AddListener<T>()
        where T : IPersistenceUnitListener, new()
    {
        listeners.Add(new T());
        return this;
    }
    public InMemoryOptions AddListener(IPersistenceUnitListener listener)
    {
        ArgumentNullException.ThrowIfNull(listener);
        listeners.Add(listener);
        return this;
    }

    #endregion
}
