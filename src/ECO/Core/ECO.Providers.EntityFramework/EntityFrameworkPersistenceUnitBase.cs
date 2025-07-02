using ECO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECO.Providers.EntityFramework;

public abstract class EntityFrameworkPersistenceUnitBase(string name, ILoggerFactory? loggerFactory = null) : PersistenceUnitBase<EntityFrameworkPersistenceUnitBase>(name, loggerFactory)
{
    #region Consts

    protected static readonly string DBCONTEXTTYPE_ATTRIBUTE = "dbContextType";

    #endregion

    #region Private_Fields

    protected Type? _DbContextType;

    protected DbContextOptions? _DbContextOptions;

    #endregion

    #region Protected_Methods

    protected abstract DbContextOptions CreateDbContextOptions(IDictionary<string, string> extendedAttributes, IConfiguration configuration);

    #endregion

    #region PersistenceUnitBase

    protected override void OnInitialize(IDictionary<string, string> extendedAttributes, IConfiguration configuration)
    {
        base.OnInitialize(extendedAttributes, configuration);
        if (extendedAttributes.TryGetValue(DBCONTEXTTYPE_ATTRIBUTE, out string? value))
        {
            _DbContextType = Type.GetType(value);
        }
        else
        {
            throw new ApplicationException(string.Format("The attribute '{0}' was not found in the persistent unit configuration", DBCONTEXTTYPE_ATTRIBUTE));
        }
        _DbContextOptions = CreateDbContextOptions(extendedAttributes, configuration);
        //Register class types
        using DbContext context = Activator.CreateInstance(_DbContextType!, _DbContextOptions) as DbContext ?? throw new InvalidCastException(nameof(context));
        foreach (var entity in context.Model.GetEntityTypes())
        {
            var entityType = entity.ClrType;
            if (entityType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAggregateRoot<>)))
                _Classes.Add(entity.ClrType);
        }
    }

    protected override IPersistenceContext OnCreateContext()
    {
        DbContext context = Activator.CreateInstance(_DbContextType!, _DbContextOptions) as DbContext ?? throw new InvalidCastException(nameof(context));
        return new EntityFrameworkPersistenceContext(context, this, _LoggerFactory?.CreateLogger<EntityFrameworkPersistenceContext>());
    }

    public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext) => new EntityFrameworkReadOnlyRepository<T, K>(dataContext);

    public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext) => new EntityFrameworkRepository<T, K>(dataContext);

    #endregion
}
