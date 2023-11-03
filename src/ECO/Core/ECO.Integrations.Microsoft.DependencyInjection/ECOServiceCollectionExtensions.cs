using ECO.Configuration;
using ECO.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ECO.Integrations.Microsoft.DependencyInjection
{
    public static class ECOServiceCollectionExtensions
    {
        public static IServiceCollection AddDataContext(
            this IServiceCollection serviceCollection,
            Action<DataContextOptions> optionsAction)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));            
            if (optionsAction == null) throw new ArgumentNullException(nameof(optionsAction));
            DataContextOptions options = new DataContextOptions();
            optionsAction(options);
            serviceCollection.AddSingleton<IPersistenceUnitFactory, PersistenceUnitFactory>(fact =>
            {
                var loggerFactory = fact.GetRequiredService<ILoggerFactory>();
                var persistenceUnitFactory = new PersistenceUnitFactory(loggerFactory);
                options.PersistenceUnitFactoryOptions(persistenceUnitFactory, loggerFactory);
                return persistenceUnitFactory;
            });
            serviceCollection.AddScoped<IDataContext>(fact =>
            {
                var persistenceUnitFactory = fact.GetRequiredService<IPersistenceUnitFactory>();
                var dataContext = persistenceUnitFactory.OpenDataContext();
                if (options.RequireTransaction) dataContext.BeginTransaction(true);
                return dataContext;
            });
            return serviceCollection;
        }

        public static IServiceCollection AddReadOnlyRepository<T,K>(this IServiceCollection serviceCollection)
            where T : class, IAggregateRoot<K>
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            serviceCollection.AddScoped<IReadOnlyRepository<T, K>>(fact =>
            {
                var persistenceUnitFactory = fact.GetRequiredService<IPersistenceUnitFactory>();
                var persistenceUnit = persistenceUnitFactory.GetPersistenceUnit<T>();
                var dataContext = fact.GetRequiredService<IDataContext>();
                return persistenceUnit.BuildReadOnlyRepository<T, K>(dataContext);
            });
            return serviceCollection;
        }

        public static IServiceCollection AddRepository<T, K>(this IServiceCollection serviceCollection)
            where T : class, IAggregateRoot<K>
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            serviceCollection.AddScoped<IRepository<T, K>>(fact =>
            {
                var persistenceUnitFactory = fact.GetRequiredService<IPersistenceUnitFactory>();
                var persistenceUnit = persistenceUnitFactory.GetPersistenceUnit<T>();
                var dataContext = fact.GetRequiredService<IDataContext>();
                return persistenceUnit.BuildRepository<T, K>(dataContext);
            });
            return serviceCollection;
        }
    }
}
