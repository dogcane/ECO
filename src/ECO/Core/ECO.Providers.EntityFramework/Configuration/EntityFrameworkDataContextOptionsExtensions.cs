using ECO.Configuration;
using ECO.Data;
using ECO.Providers.EntityFramework.Utils;
using ECO.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Providers.EntityFramework.Configuration
{
    public static class EntityFrameworkDataContextOptionsExtensions
    {
        public static DataContextOptions UseEntityFramework<T>(this DataContextOptions dataContextOptions, string persistenceUnitName, Action<EntityFrameworkOptions> optionsAction)
            where T : DbContext
        {
            if (dataContextOptions == null) throw new ArgumentNullException(nameof(dataContextOptions));
            if (string.IsNullOrWhiteSpace(persistenceUnitName)) throw new ArgumentNullException(nameof(persistenceUnitName));
            if (optionsAction == null) throw new ArgumentNullException(nameof(optionsAction));
            dataContextOptions.PersistenceUnitFactoryOptions += (persistenceUnitFactory, loggerFactory) =>
            {
                EntityFrameworkOptions options = new();
                optionsAction(options);
                EntityFrameworkPersistenceUnit<T> persistenceUnit = new(persistenceUnitName, options.DbContextOptions.Options, loggerFactory);
                foreach (var entityType in DbContextFacade<T>.GetAggregateTypesFromDBContext(options.DbContextOptions.Options))
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
        private readonly List<IPersistenceUnitListener> listeners = new();
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
            if (listener == null) throw new ArgumentNullException(nameof(listener));
            listeners.Add(listener);
            return this;
        }
        #endregion
    }
}
