namespace ECO.Providers.EntityFramework;

using ECO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

/// <summary>
/// Generic Entity Framework persistence unit that works with a specific DbContext type.
/// Provides type-safe creation of Entity Framework persistence contexts and repositories.
/// </summary>
/// <typeparam name="TDbContext">The type of DbContext this persistence unit manages.</typeparam>
/// <param name="name">The name of this persistence unit.</param>
/// <param name="dbContextOptions">The DbContext options for creating DbContext instances.</param>
/// <param name="loggerFactory">Optional logger factory for creating loggers.</param>
public class EntityFrameworkPersistenceUnit<TDbContext>(
    string name, 
    DbContextOptions dbContextOptions, 
    ILoggerFactory? loggerFactory = null)
    : PersistenceUnitBase<EntityFrameworkPersistenceUnitBase>(name, loggerFactory)
    where TDbContext : DbContext
{
    #region Protected_Fields    
    /// <summary>
    /// The DbContext options used to create DbContext instances.
    /// </summary>
    protected readonly DbContextOptions _dbContextOptions = dbContextOptions ?? throw new ArgumentNullException(nameof(dbContextOptions));    
    #endregion

    #region PersistenceUnitBase
    
    /// <inheritdoc />
    protected override IPersistenceContext OnCreateContext()
    {
        var context = Activator.CreateInstance(typeof(TDbContext), _dbContextOptions) as DbContext;
        return context is not null
            ? new EntityFrameworkPersistenceContext(context, this, _LoggerFactory?.CreateLogger<EntityFrameworkPersistenceContext>())
            : throw new InvalidOperationException($"Failed to create instance of {typeof(TDbContext).Name}. Ensure it has a constructor that accepts {nameof(DbContextOptions)}.");
    }

    /// <inheritdoc />
    public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext)
        => new EntityFrameworkReadOnlyRepository<T, K>(dataContext);

    /// <inheritdoc />
    public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext)
        => new EntityFrameworkRepository<T, K>(dataContext);

    #endregion
}
