namespace ECO.Providers.InMemory;

using ECO.Data;
using Microsoft.Extensions.Logging;

public class InMemoryPersistenceContext(IPersistenceUnit persistenceUnit, ILogger<InMemoryPersistenceContext>? logger)
    : PersistenceContextBase<InMemoryPersistenceContext>(persistenceUnit, logger)
{
}
