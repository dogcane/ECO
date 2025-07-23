namespace ECO.Providers.EntityFramework;

using ECO.Data;
using ECO.Providers.EntityFramework.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Abstract base class for Entity Framework persistence units that require configuration-based initialization.
/// Provides common functionality for creating DbContext instances from configuration.
/// </summary>
/// <param name="name">The name of this persistence unit.</param>
/// <param name="loggerFactory">Optional logger factory for creating loggers.</param>
public abstract class EntityFrameworkPersistenceUnitBase(string name, ILoggerFactory? loggerFactory = null)
    : PersistenceUnitBase<EntityFrameworkPersistenceUnitBase>(name, loggerFactory)
{
    #region Consts    
    /// <summary>
    /// The configuration attribute name for specifying the DbContext type.
    /// </summary>
    protected static readonly string DBCONTEXTTYPE_ATTRIBUTE = "dbContextType";    
    #endregion

    #region Protected_Fields    
    /// <summary>
    /// The Type of the DbContext to be instantiated.
    /// </summary>
    protected Type? _dbContextType;
    
    /// <summary>
    /// The DbContext options for creating DbContext instances.
    /// </summary>
    protected DbContextOptions? _dbContextOptions;
    #endregion

    #region Protected_Methods
    
    /// <summary>
    /// Creates the DbContext options from the provided configuration attributes.
    /// This method must be implemented by derived classes to provide provider-specific options.
    /// </summary>
    /// <param name="extendedAttributes">The extended attributes from configuration.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The configured DbContextOptions.</returns>
    protected abstract DbContextOptions CreateDbContextOptions(IDictionary<string, string> extendedAttributes, IConfiguration configuration);
    
    #endregion

    #region PersistenceUnitBase
    
    /// <inheritdoc />
    protected override void OnInitialize(IDictionary<string, string> extendedAttributes, IConfiguration configuration)
    {
        base.OnInitialize(extendedAttributes, configuration);
        
        // Get the DbContext type from configuration
        if (extendedAttributes.TryGetValue(DBCONTEXTTYPE_ATTRIBUTE, out string? dbContextTypeName))
        {
            _dbContextType = Type.GetType(dbContextTypeName) ??
                throw new InvalidOperationException($"Could not resolve DbContext type: {dbContextTypeName}");
        }
        else
        {
            throw new InvalidOperationException($"The required attribute '{DBCONTEXTTYPE_ATTRIBUTE}' was not found in the persistence unit configuration.");
        }
        
        // Create DbContext options
        _dbContextOptions = CreateDbContextOptions(extendedAttributes, configuration);
        
        // Discover and register aggregate root types from the DbContext
        foreach(var aggregate in DbContextFacade.GetAggregateTypesFromDBContext(_dbContextType))
        {
            AddClass(aggregate);
        }
    }

    /// <inheritdoc />
    protected override IPersistenceContext OnCreateContext()
    {
        if (_dbContextType is null || _dbContextOptions is null)
        {
            throw new InvalidOperationException($"Persistence unit '{Name}' has not been properly initialized. Call Initialize() first.");
        }
        
        var context = Activator.CreateInstance(_dbContextType, _dbContextOptions) as DbContext;
        return context is not null
            ? new EntityFrameworkPersistenceContext(context, this, _LoggerFactory?.CreateLogger<EntityFrameworkPersistenceContext>())
            : throw new InvalidOperationException($"Failed to create instance of {_dbContextType.Name}. Ensure it has a constructor that accepts {nameof(DbContextOptions)}.");
    }

    /// <inheritdoc />
    public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext)
        => new EntityFrameworkReadOnlyRepository<T, K>(dataContext);

    /// <inheritdoc />
    public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext)
        => new EntityFrameworkRepository<T, K>(dataContext);

    #endregion
}
