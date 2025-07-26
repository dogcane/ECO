using ECO.Configuration;
using ECO.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ECO.Integrations.Microsoft.DependencyInjection;

public static class ECOServiceCollectionExtensions
{
    public static IServiceCollection AddDataContext(
        this IServiceCollection serviceCollection,
        Action<DataContextOptions> optionsAction)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        ArgumentNullException.ThrowIfNull(optionsAction);
        var options = new DataContextOptions
        {
            PersistenceUnitFactoryOptions = (_, _) => { }
        };
        optionsAction(options);
        serviceCollection.AddSingleton<IPersistenceUnitFactory, PersistenceUnitFactory>(fact =>
        {
            var loggerFactory = fact.GetRequiredService<ILoggerFactory>();
            var persistenceUnitFactory = new PersistenceUnitFactory(loggerFactory);
            options.PersistenceUnitFactoryOptions(persistenceUnitFactory, loggerFactory);
            return persistenceUnitFactory;
        });
        serviceCollection.AddScoped(fact =>
        {
            var persistenceUnitFactory = fact.GetRequiredService<IPersistenceUnitFactory>();
            var dataContext = persistenceUnitFactory.OpenDataContext();
            if (options.RequireTransaction) dataContext.BeginTransaction(true);
            return dataContext;
        });
        return serviceCollection;
    }

    public static IServiceCollection AddReadOnlyRepository<T, K>(this IServiceCollection serviceCollection)
        where T : class, IAggregateRoot<K>
        where K : notnull, IEquatable<K>
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        serviceCollection.AddScoped(fact =>
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
        where K : notnull, IEquatable<K>
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        serviceCollection.AddScoped(fact =>
        {
            var persistenceUnitFactory = fact.GetRequiredService<IPersistenceUnitFactory>();
            var persistenceUnit = persistenceUnitFactory.GetPersistenceUnit<T>();
            var dataContext = fact.GetRequiredService<IDataContext>();
            return persistenceUnit.BuildRepository<T, K>(dataContext);
        });
        return serviceCollection;
    }
}
