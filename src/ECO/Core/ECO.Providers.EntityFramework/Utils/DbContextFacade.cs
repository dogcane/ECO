using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECO.Providers.EntityFramework.Utils;

internal class DbContextFacade<T>
    where T : DbContext
{
    public static IEnumerable<Type> GetAggregateTypesFromDBContext() => [.. typeof(T)
            .GetProperties()
            .Where(prop => prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
            .Select(prop => prop.PropertyType.GetGenericArguments()[0])
            .Where(entityType => entityType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAggregateRoot<>)))];
}
