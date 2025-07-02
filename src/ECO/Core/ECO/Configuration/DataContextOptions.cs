using ECO.Data;
using Microsoft.Extensions.Logging;

namespace ECO.Configuration;

/// <summary>
/// Options for configuring the data context and persistence unit factory.
/// </summary>
public class DataContextOptions
{
    /// <summary>
    /// Delegate to configure the persistence unit factory with logging support.
    /// </summary>
    public required Action<IPersistenceUnitFactory, ILoggerFactory> PersistenceUnitFactoryOptions { get; set; } = (persistenceUnitFactory, loggerFactory) => { };

    /// <summary>
    /// Indicates whether a transaction is required for operations.
    /// </summary>
    public bool RequireTransaction { get; set; } = false;
}
