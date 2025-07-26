# ECO MongoDB Provider - Fluent Configuration

The ECO MongoDB provider now supports fluent configuration, allowing you to configure MongoDB persistence units programmatically without requiring configuration files.

## Usage Examples

### Fluent Configuration (Recommended)

```csharp
using ECO.Providers.MongoDB.Configuration;

builder.Services.AddDataContext(options =>
{
    options.UseMongoDB("ecosampleapp.mongodb", opt =>
    {
        // Required settings
        opt.ConnectionString = "mongodb://localhost:27017";
        opt.DatabaseName = "ECOSampleApp";
        
        // Auto-discover aggregate types from assembly
        opt.AddAssemblyFromType<ECO.Sample.Domain.AssemblyMarker>();
        
        // Or add specific aggregate types
        opt.AddAggregateType<Event>();
        opt.AddAggregateType<Speaker>();
        
        // Optional: Configure mapping assemblies
        opt.MappingAssemblies = "ECO.Sample.Infrastructure.DAL.MongoDB";
        
        // Optional: Enable identity map for serializers
        opt.UseIdentityMap = true;
        
        // Optional: Add persistence unit listeners
        opt.AddListener<MyCustomListener>();
    });
});
```

### Configuration File (Legacy Support)

The provider maintains backward compatibility with configuration files:

```csharp
// In Program.cs
builder.Configuration.AddJsonFile("ecosettings.mongodb.json");
builder.Services.AddDataContext(options =>
{
    options.UseConfiguration(builder.Configuration);
});
```

```json
// ecosettings.mongodb.json
{
  "eco": {
    "persistenceUnits": [
      {
        "name": "ecosampleapp.mongodb",
        "type": "ECO.Providers.MongoDB.MongoPersistenceUnit, ECO.Providers.MongoDB",
        "classes": [
          "ECO.Sample.Domain.Event, ECO.Sample.Domain",
          "ECO.Sample.Domain.Speaker, ECO.Sample.Domain"
        ],
        "attributes": {
          "connectionString": "mongodb://localhost:27017",
          "database": "ECOSampleApp",
          "mappingAssemblies": "ECO.Sample.Infrastructure.DAL.MongoDB"
        }
      }
    ]
  }
}
```

## API Reference

### MongoDBOptions Methods

- `ConnectionString` - Sets the MongoDB connection string (required)
- `DatabaseName` - Sets the MongoDB database name (required)
- `MappingAssemblies` - Sets semicolon-separated mapping assembly names (optional)
- `UseIdentityMap` - Enables identity map for serializers (optional, default: false)
- `AddAssemblyFromType<T>()` - Auto-discovers aggregate types from the assembly containing type T
- `AddAssemblyFromType(Type)` - Auto-discovers aggregate types from the assembly containing the specified type
- `AddAggregateType<T>()` - Adds a specific aggregate type
- `AddAggregateType(Type)` - Adds a specific aggregate type
- `AddListener<T>()` - Adds a persistence unit listener of type T
- `AddListener(IPersistenceUnitListener)` - Adds a persistence unit listener instance

### Benefits of Fluent Configuration

1. **Type Safety** - Compile-time checking of configuration
2. **IntelliSense Support** - Full IDE support with auto-completion
3. **No External Files** - Configuration is part of your code
4. **Environment-Specific** - Easy to vary configuration by environment
5. **Refactoring Friendly** - Configuration changes when you rename types