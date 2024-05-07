using ECO.Data;
using Microsoft.Extensions.Logging;

namespace ECO.Providers.InMemory;

public class InMemoryPersistenceContext(IPersistenceUnit persistenceUnit, ILogger<InMemoryPersistenceContext>? logger) 
    : PersistenceContextBase<InMemoryPersistenceContext>(persistenceUnit, logger)
{

}
