using ECO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace ECO.Providers.EntityFramework;

public class EntityFrameworkPersistenceUnit<TDbContext>(string name, DbContextOptions dbContextOptions, ILoggerFactory? loggerFactory = null) : PersistenceUnitBase<EntityFrameworkPersistenceUnitBase>(name, loggerFactory)
    where TDbContext : DbContext
{
    #region Private_Fields
    protected readonly DbContextOptions _DbContextOptions = dbContextOptions ?? throw new ArgumentNullException(nameof(dbContextOptions));
    #endregion

    #region PersistenceUnitBase

    protected override IPersistenceContext OnCreateContext()
    {
        DbContext context = Activator.CreateInstance(typeof(TDbContext), _DbContextOptions) as DbContext ?? throw new InvalidCastException(nameof(context));
        return new EntityFrameworkPersistenceContext(context, this, _LoggerFactory?.CreateLogger<EntityFrameworkPersistenceContext>());
    }

    public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext) => new EntityFrameworkReadOnlyRepository<T, K>(dataContext);

    public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext) => new EntityFrameworkRepository<T, K>(dataContext);

    #endregion
}
