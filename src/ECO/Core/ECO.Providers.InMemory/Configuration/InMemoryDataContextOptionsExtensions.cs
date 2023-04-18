using ECO.Configuration;
using ECO.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECO.Providers.InMemory.Configuration
{
    public static class InMemoryDataContextOptionsExtensions
    {
        public static DataContextOptions UseInMemory(this DataContextOptions dataContextOptions, Action<InMemoryOptions> optionsAction)
        {            
            if (dataContextOptions == null) throw new ArgumentNullException(nameof(dataContextOptions));
            if (optionsAction == null) throw new ArgumentNullException(nameof(optionsAction));
            dataContextOptions.PersistenceUnitFactoryOptions += (persistenceUnitFactory, loggerFactory) =>
            {
                InMemoryOptions options = new InMemoryOptions { };
                optionsAction(options);
                InMemoryPersistenceUnit persistenceUnit = new InMemoryPersistenceUnit(options.Name, loggerFactory);
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
        public string Name { get; set; } = string.Empty;

        public Type[] Classes { get; set; } = new Type[0];

        public IPersistenceUnitListener[] Listeners { get; set; } = new IPersistenceUnitListener[0];
    }
}
