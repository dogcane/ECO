﻿using ECO.Data;
using Microsoft.Extensions.Logging;

namespace ECO.Configuration;

public class DataContextOptions
{
    public required Action<IPersistenceUnitFactory, ILoggerFactory> PersistenceUnitFactoryOptions { get; set; } = (persistenceUnitFactory, loggerFactory) => { };
    public bool RequireTransaction { get; set; } = false;
}
