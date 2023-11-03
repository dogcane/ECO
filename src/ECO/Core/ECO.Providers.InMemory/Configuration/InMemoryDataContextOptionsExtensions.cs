using ECO.Configuration;
using ECO.Data;
using ECO.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ECO.Providers.InMemory.Configuration
{
    public static class InMemoryDataContextOptionsExtensions
    {
        public static DataContextOptions UseInMemory(this DataContextOptions dataContextOptions, string persistenceUnitName, Action<InMemoryOptions> optionsAction)
        {            
            if (dataContextOptions == null) throw new ArgumentNullException(nameof(dataContextOptions));
            if (string.IsNullOrWhiteSpace(persistenceUnitName)) throw new ArgumentNullException(nameof(persistenceUnitName));
            if (optionsAction == null) throw new ArgumentNullException(nameof(optionsAction));
            dataContextOptions.PersistenceUnitFactoryOptions += (persistenceUnitFactory, loggerFactory) =>
            {
                InMemoryOptions options = new InMemoryOptions { };
                optionsAction(options);
                InMemoryPersistenceUnit persistenceUnit = new InMemoryPersistenceUnit(persistenceUnitName, loggerFactory);
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
        private readonly List<Assembly> assemblies = new();
        private readonly List<Type> classes = new();
        private readonly List<IPersistenceUnitListener> listeners = new();
        #endregion

        #region Properties
        public IEnumerable<Assembly> Assemblies => assemblies;
        public IEnumerable<Type> Classes => classes;
        public IEnumerable<IPersistenceUnitListener> Listeners => listeners;
        #endregion

        #region Methods
        public InMemoryOptions AddAssembly(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            assemblies.Add(assembly);
            return this;
        }
        public InMemoryOptions AddAssemblyFromType<T>()
        {
            return AddAssembly(typeof(T).Assembly);
        }
        public InMemoryOptions AddClass<T, K>()
            where T : class, IAggregateRoot<K>
        {
            return AddClass(typeof(T));
        }
        public InMemoryOptions AddClass(Type @class)
        {
            if (@class == null) throw new ArgumentNullException(nameof(@class));
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
            if (listener == null) throw new ArgumentNullException(nameof(listener));
            listeners.Add(listener);
            return this;
        }

        #endregion
    }
}
