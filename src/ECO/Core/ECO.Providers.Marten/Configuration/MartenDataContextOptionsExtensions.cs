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
        public static DataContextOptions UseMarten(this DataContextOptions dataContextOptions, Action<MartenOptions> optionsAction)
        {
            if (dataContextOptions == null) throw new ArgumentNullException(nameof(dataContextOptions));
            if (optionsAction == null) throw new ArgumentNullException(nameof(optionsAction));
            dataContextOptions.PersistenceUnitFactoryOptions += (persistenceUnitFactory, loggerFactory) =>
            {
                MartenOptions options = new MartenOptions();
                optionsAction(options);
                options.StoreOptions.Policies.OnDocuments(new ECODocumentPolicy());
                DocumentStore store = new DocumentStore(options.StoreOptions);
                MartenPersistenceUnit persistenceUnit = new MartenPersistenceUnit(options.Name, loggerFactory, store);
                foreach(var @class in options.Assemblies.SelectMany(asm => asm.ExportedTypes).OfAggregateRootType())
                {
                    persistenceUnit.AddClass(@class);
                }
                foreach(var @class in options.Classes)
                {
                    persistenceUnit.AddClass(@class);
                }                
                foreach(var listener in options.Listeners)
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
        public string Name { get; set; } = string.Empty;

        public Assembly[] Assemblies { get; set; } = new Assembly[0];

        public Type[] Classes { get; set; } = new Type[0];

        public IPersistenceUnitListener[] Listeners { get; set; } = new IPersistenceUnitListener[0];

        public StoreOptions StoreOptions { get; set;} = new StoreOptions();
    }
}
