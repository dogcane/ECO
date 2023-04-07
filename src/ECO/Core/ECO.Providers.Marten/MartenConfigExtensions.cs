using ECO.Configuration;
using ECO.Integrations.Microsoft.DependencyInjection;
using Marten;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECO.Providers.Marten
{
    public static class MartenConfigExtensions
    {
        public static DataContextOptions UseMarten(this DataContextOptionsExtended options, string connectionString)
        {
            options.Services.AddMarten(connectionString);
            return options;
        }

        public static DataContextOptions UseMarten(this DataContextOptionsExtended options, StoreOptions storeOptions)
        {
            options.Services.AddMarten(storeOptions);
            return options;
        }

        public static DataContextOptions UseMarten(this DataContextOptionsExtended options, Action<StoreOptions> optionsAction)
        {
            StoreOptions storeOptions = new StoreOptions();
            optionsAction(storeOptions);
            options.Services.AddMarten(storeOptions);
            return options;
        }
    }
}
