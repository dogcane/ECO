using ECO.Configuration;
using ECO.Data;
using ECO.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
        public static DataContextOptions UseEntityFramework<T>(this DataContextOptions dataContextOptions, Action<EntityFrameworkOptions> optionsAction)
            where T : DbContext
        {
            if (dataContextOptions == null) throw new ArgumentNullException(nameof(dataContextOptions));
            if (optionsAction == null) throw new ArgumentNullException(nameof(optionsAction));
            dataContextOptions.PersistenceUnitFactoryOptions += (persistenceUnitFactory, loggerFactory) =>
            {
                EntityFrameworkOptions options = new();
                optionsAction(options);
                EntityFrameworkPersistenceUnit<T> persistenceUnit = new(options.Name, options.DbContextOptions.Options, loggerFactory);                
                using DbContext context = Activator.CreateInstance(typeof(T), options.DbContextOptions.Options) as DbContext ?? throw new InvalidCastException(nameof(context));
                foreach (var entity in context.Model.GetEntityTypes())
                {
                    var entityType = entity.ClrType;
                    if (entityType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAggregateRoot<>)))
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
        public string Name { get; set; } = string.Empty;

        public IPersistenceUnitListener[] Listeners { get; set; } = new IPersistenceUnitListener[0];

        public DbContextOptionsBuilder DbContextOptions { get; set; } = new DbContextOptionsBuilder();
    }
}
