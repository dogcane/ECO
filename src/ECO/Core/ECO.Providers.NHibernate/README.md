# NHibernate Configuration Extension Usage

This document shows how to use the new FluentNHibernate configuration extension for ECO.

## Basic Usage

```csharp
using ECO.Configuration;
using ECO.Providers.NHibernate.Configuration;
using FluentNHibernate.Cfg.Db;

// Configure ECO data context with NHibernate
var dataContextOptions = new DataContextOptions()
    .UseNHibernate("MyPersistenceUnit", options =>
    {
        // Configure database
        options.UseDatabase(SQLiteConfiguration.Standard
            .UsingFile("database.db"));
        
        // Add mapping assemblies
        options.AddMappingAssembly("MyDomain.Mappings");
        
        // Configure FluentNHibernate mappings
        options.ConfigureMappings(m =>
        {
            m.FluentMappings.AddFromAssemblyOf<MyEntity>();
            m.HbmMappings.AddFromAssemblyOf<MyEntity>();
        });
        
        // Add persistence unit listeners
        options.AddListener<MyCustomListener>();
        
        // Set session interceptor
        options.UseSessionInterceptor<MyInterceptor>();
        
        // Set configuration properties
        options.SetProperty("hibernate.show_sql", "true");
        options.SetProperty("hibernate.format_sql", "true");
        
        // Custom NHibernate configuration
        options.ConfigureNHibernate(cfg =>
        {
            cfg.SetProperty("hibernate.cache.use_second_level_cache", "true");
        });
    });
```

## SQL Server Example

```csharp
options.UseDatabase(MsSqlConfiguration.MsSql2012
    .ConnectionString(c => c.FromConnectionStringWithKey("DefaultConnection"))
    .ShowSql());
```

## PostgreSQL Example

```csharp
options.UseDatabase(PostgreSQLConfiguration.Standard
    .ConnectionString("Host=localhost;Database=mydb;Username=user;Password=pass"));
```

## Multiple Assemblies

```csharp
options.AddMappingAssemblies(
    typeof(UserMapping).Assembly,
    typeof(OrderMapping).Assembly);
```

## Advanced Configuration

```csharp
options
    .UseDatabase(SQLiteConfiguration.Standard.InMemory())
    .AddMappingAssembly("Domain.Mappings")
    .SetProperty("hibernate.hbm2ddl.auto", "create-drop")
    .ConfigureMappings(m =>
    {
        m.FluentMappings
            .AddFromAssemblyOf<User>()
            .Conventions.Add<MyCustomConvention>();
    })
    .ConfigureNHibernate(cfg =>
    {
        cfg.SetProperty("hibernate.query.substitutions", "true 1, false 0, yes 'Y', no 'N'");
    });
```

## Backward Compatibility

The simplified `NHPersistenceUnit` still supports the legacy configuration method through extended attributes, ensuring existing code continues to work without changes.

## Migration from Legacy Configuration

### Before (Legacy)
```xml
<persistenceUnit name="MyUnit">
  <property name="connection.connection_string" value="..." />
  <property name="mappingAssemblies" value="Assembly1;Assembly2" />
  <property name="sessionInterceptor" value="MyApp.MyInterceptor" />
</persistenceUnit>
```

### After (FluentNHibernate)
```csharp
options
    .UseDatabase(SQLiteConfiguration.Standard.UsingFile("database.db"))
    .AddMappingAssemblies("Assembly1", "Assembly2")
    .UseSessionInterceptor<MyInterceptor>();
```

The new approach provides:
- Type safety
- Better IntelliSense support
- Fluent configuration API
- Integration with FluentNHibernate features
- Simplified setup and maintenance
