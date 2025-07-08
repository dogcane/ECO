namespace ECO.Providers.InMemory;

using ECO.Data;
using Microsoft.Extensions.Logging;

public sealed class InMemoryPersistenceUnit(string name, ILoggerFactory loggerFactory)
    : PersistenceUnitBase<InMemoryPersistenceUnit>(name, loggerFactory)
{
    #region Methods

    protected override IPersistenceContext OnCreateContext() =>
        new InMemoryPersistenceContext(this, _LoggerFactory?.CreateLogger<InMemoryPersistenceContext>());

    public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext) =>
        new InMemoryReadOnlyRepository<T, K>(dataContext);

    public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext) =>
        new InMemoryRepository<T, K>(dataContext);

    #endregion
}
