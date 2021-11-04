using ECO.Configuration;
using ECO.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECO.Web.Infrastructure
{
    public static class ECOServiceCollectionExtensions
    {
        public static IServiceCollection AddECODataContext(
            this IServiceCollection serviceCollection,
            IConfiguration config) => AddECODataContext(serviceCollection, config, options => { });

        public static IServiceCollection AddECODataContext(
            this IServiceCollection serviceCollection,
            IConfiguration config,
            Action<DataContextOptions> optionsAction)

        {
            serviceCollection.Configure<ECOOptions>(settings => {
                config.GetSection(ECOOptions.ECOConfigurationName).Bind(settings);
            });
            serviceCollection.AddSingleton<IPersistenceUnitFactory, PersistenceUnitFactory>();
            DataContextOptions options = new DataContextOptions();
            optionsAction(options);            
            serviceCollection.AddScoped<IDataContext>(fact =>
            {
                var persistenceUnitFactory = fact.GetRequiredService<IPersistenceUnitFactory>();
                var dataContext = new DataContext(persistenceUnitFactory);
                if (options.RequireTransaction) dataContext.BeginTransaction(true);
                return dataContext;
            });
            return serviceCollection;
        }
    }
}
