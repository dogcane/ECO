namespace ECO.Providers.EntityFramework;

using ECO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

public class EntityFrameworkPersistenceUnit<TDbContext>(string name, DbContextOptions dbContextOptions, ILoggerFactory? loggerFactory = null)
    : PersistenceUnitBase<EntityFrameworkPersistenceUnitBase>(name, loggerFactory)
    where TDbContext : DbContext
{
    #region Private_Fields
    protected readonly DbContextOptions _DbContextOptions = dbContextOptions ?? throw new ArgumentNullException(nameof(dbContextOptions));
    #endregion

    #region PersistenceUnitBase
    protected override IPersistenceContext OnCreateContext() =>
        Activator.CreateInstance(typeof(TDbContext), _DbContextOptions) is DbContext context
            ? new EntityFrameworkPersistenceContext(context, this, _LoggerFactory?.CreateLogger<EntityFrameworkPersistenceContext>())
            : throw new InvalidCastException(nameof(context));

    public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext)
        => new EntityFrameworkReadOnlyRepository<T, K>(dataContext);

    public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext)
        => new EntityFrameworkRepository<T, K>(dataContext);
    #endregion
}
