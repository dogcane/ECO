using ECO.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ECO.Configuration
{
    public static class ECOServiceCollectionExtensions
    {

        public static IServiceCollection AddDataContext(
            this IServiceCollection serviceCollection,
            Action<DataContextOptions> optionsAction)
        {
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
                var logger = fact.GetRequiredService<ILogger<DataContext>>();
                var dataContext = new DataContext(persistenceUnitFactory, logger);
                if (options.RequireTransaction) dataContext.BeginTransaction(true);
                return dataContext;
            });
            return serviceCollection;
        }
    }
}
