using ECO.Data;
using Microsoft.Extensions.Logging;
using System;

namespace ECO.Configuration
{
    public sealed class DataContextOptions
    {
        public Action<IPersistenceUnitFactory, ILoggerFactory> PersistenceUnitFactoryOptions { get; set; } = (persistenceUnitFactory, loggerFactory) => { };
        public bool RequireTransaction { get; set; } = false;
    }
}
