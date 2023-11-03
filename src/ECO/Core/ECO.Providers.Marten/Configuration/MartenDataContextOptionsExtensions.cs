using ECO.Configuration;
using ECO.Data;
using ECO.Integrations.Microsoft.DependencyInjection;
using ECO.Utils;
using Marten;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ECO.Providers.Marten.Configuration
{
    public static class MartenDataContextOptionsExtensions
    {
        public static DataContextOptions UseMarten(this DataContextOptions dataContextOptions, string persistenceUnitName, Action<MartenOptions> optionsAction)
        {
            if (dataContextOptions == null) throw new ArgumentNullException(nameof(dataContextOptions));
            if (string.IsNullOrWhiteSpace(persistenceUnitName)) throw new ArgumentNullException(nameof(persistenceUnitName));
            if (optionsAction == null) throw new ArgumentNullException(nameof(optionsAction));
            dataContextOptions.PersistenceUnitFactoryOptions += (persistenceUnitFactory, loggerFactory) =>
            {
                MartenOptions options = new MartenOptions();
                optionsAction(options);
                options.StoreOptions.Policies.OnDocuments(new ECODocumentPolicy());
                DocumentStore store = new DocumentStore(options.StoreOptions);
                MartenPersistenceUnit persistenceUnit = new MartenPersistenceUnit(persistenceUnitName, loggerFactory, store);
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
        private readonly List<Assembly> assemblies = new();
        private readonly List<Type> classes = new();
        private readonly List<IPersistenceUnitListener> listeners = new();
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
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
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
            if (@class == null) throw new ArgumentNullException(nameof(@class));
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
            if (listener == null) throw new ArgumentNullException(nameof(listener));
            listeners.Add(listener);
            return this;
        }

        #endregion
    }
}
