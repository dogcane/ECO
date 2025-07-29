# MongoDB Provider

The MongoDB provider integrates ECO with MongoDB, providing document-based persistence for aggregate roots. It supports both fluent configuration and traditional configuration file approaches.

## Features

- **Document Storage**: Store aggregates as documents in MongoDB collections
- **Fluent Configuration**: Type-safe, IntelliSense-supported configuration 
- **Auto-Discovery**: Automatically discover aggregate types from assemblies
- **Identity Mapping**: Optional identity map support for serializers
- **Custom Mapping**: Support for custom BSON serializers and conventions
- **Event Listeners**: Extensible through persistence unit listeners

## Installation

Add the MongoDB provider package to your project:

```bash
dotnet add package ECO.Providers.MongoDB
```

```xml
<PackageReference Include="ECO.Providers.MongoDB" Version="3.0.0" />
```

## Configuration

### Fluent Configuration (Recommended)

Configure MongoDB using the fluent API in your startup:

~~~ c#
using ECO.Providers.MongoDB.Configuration;

builder.Services.AddDataContext(options =>
{
    options.UseMongoDB("ecosampleapp.mongodb", opt =>
    {
        // Required settings
        opt.ConnectionString = "mongodb://localhost:27017";
        opt.DatabaseName = "ECOSampleApp";
        
        // Auto-discover aggregate types from assembly
        opt.AddAssemblyFromType<Event>();
        
        // Or add specific aggregate types
        opt.AddAggregateType<Event>();
        opt.AddAggregateType<Speaker>();
        opt.AddAggregateType<Session>();
        
        // Optional: Configure mapping assemblies
        opt.MappingAssemblies = "ECO.Sample.Infrastructure.DAL.MongoDB";
        
        // Optional: Enable identity map for serializers
        opt.UseIdentityMap = true;
        
        // Optional: Add persistence unit listeners
        opt.AddListener<MyCustomListener>();
    });
});
~~~

### Configuration File (Legacy Support)

You can also use traditional configuration files:

~~~ json
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
~~~

Register the configuration:

~~~ c#
builder.Configuration.AddJsonFile("ecosettings.mongodb.json");
builder.Services.AddDataContext(options =>
{
    options.UseConfiguration(builder.Configuration);
});
~~~

## Repository Implementation

Create repositories by inheriting from `MongoRepository<T, K>`:

~~~ c#
using ECO.Data;
using ECO.Providers.MongoDB;

public class EventMongoRepository : MongoRepository<Event, Guid>
{
    public EventMongoRepository(IDataContext dataContext) : base(dataContext)
    {
    }

    public async Task<IEnumerable<Event>> FindByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var filter = Builders<Event>.Filter.And(
            Builders<Event>.Filter.Gte(e => e.StartDate, startDate),
            Builders<Event>.Filter.Lte(e => e.EndDate, endDate)
        );
        
        return await Collection.Find(filter).ToListAsync();
    }
}
~~~

## Data Storage

### Document Structure

Aggregates are stored as BSON documents in MongoDB collections:

~~~ c#
// Event aggregate stored as:
{
  "_id": ObjectId("..."),
  "Id": "550e8400-e29b-41d4-a716-446655440000",
  "Title": "Azure Conference 2024",
  "Description": "Annual Azure conference",
  "StartDate": ISODate("2024-03-15T09:00:00Z"),
  "EndDate": ISODate("2024-03-17T17:00:00Z"),
  "Status": "Published"
}
~~~

### Collection Naming

By default, collections are named after the aggregate type. You can customize this:

~~~ c#
public class EventMongoRepository : MongoRepository<Event, Guid>
{
    public EventMongoRepository(IDataContext dataContext) 
        : base("events", dataContext) // Custom collection name
    {
    }
}
~~~

## Querying

### MongoDB Driver Queries

Access the native MongoDB collection for complex queries:

~~~ c#
public class EventMongoRepository : MongoRepository<Event, Guid>
{
    public async Task<IEnumerable<Event>> FindPublishedEventsAsync()
    {
        var filter = Builders<Event>.Filter.Eq(e => e.Status, EventStatus.Published);
        return await Collection.Find(filter).ToListAsync();
    }

    public async Task<long> CountEventsByLocationAsync(string location)
    {
        var filter = Builders<Event>.Filter.Eq(e => e.Location, location);
        return await Collection.CountDocumentsAsync(filter);
    }
}
~~~

### Aggregation Pipeline

Use MongoDB's aggregation framework:

~~~ c#
public async Task<IEnumerable<EventSummary>> GetEventSummariesAsync()
{
    var pipeline = new BsonDocument[]
    {
        new("$group", new BsonDocument
        {
            { "_id", "$Location" },
            { "count", new BsonDocument("$sum", 1) },
            { "avgDuration", new BsonDocument("$avg", new BsonDocument("$subtract", new BsonArray { "$EndDate", "$StartDate" })) }
        })
    };

    return await Collection.Aggregate<EventSummary>(pipeline).ToListAsync();
}
~~~

## Common Patterns

### Repository with Custom Methods

~~~ c#
public interface IEventMongoRepository : IRepository<Event, Guid>
{
    Task<IEnumerable<Event>> FindByLocationAsync(string location);
    Task<Event?> FindBySlugAsync(string slug);
}

public class EventMongoRepository : MongoRepository<Event, Guid>, IEventMongoRepository
{
    public EventMongoRepository(IDataContext dataContext) : base(dataContext) { }

    public async Task<IEnumerable<Event>> FindByLocationAsync(string location)
    {
        var filter = Builders<Event>.Filter.Eq(e => e.Location, location);
        return await Collection.Find(filter).ToListAsync();
    }

    public async Task<Event?> FindBySlugAsync(string slug)
    {
        var filter = Builders<Event>.Filter.Eq(e => e.Slug, slug);
        return await Collection.Find(filter).FirstOrDefaultAsync();
    }
}
~~~

## See Also

- [Providers Overview](Providers.md) - Overview of all available providers
- [Getting Started](Getting-started.md) - Basic ECO concepts and setup
- [Repositories](Repositories.md) - Repository pattern implementation
- [Providers-EF](Providers-EF.md) - Entity Framework Core provider
- [Providers-InMemory](Providers-InMemory.md) - In-Memory provider for testing
