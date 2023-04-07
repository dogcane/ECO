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
            Action<DataContextOptionsExtended> optionsAction)
        {
            DataContextOptionsExtended options = new DataContextOptionsExtended(serviceCollection);
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
    }
}
