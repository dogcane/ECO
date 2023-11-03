using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECO.Providers.EntityFramework.Utils;

internal class DbContextFacade<T>
    where T : DbContext
{
    public static IEnumerable<Type> GetAggregateTypesFromDBContext(DbContextOptions options)
    {
        return typeof(T)
            .GetProperties()
            .Where(prop => prop.PropertyType.IsGenericType &&
                            prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
            .Select(prop => prop.PropertyType.GetGenericArguments()[0])
            .Where(entityType => entityType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAggregateRoot<>)))
            .ToArray();
        /*
        using DbContext context = Activator.CreateInstance(typeof(T), options) as DbContext ?? throw new InvalidCastException(nameof(context));
        foreach (var entity in context.Model.GetEntityTypes())
        {
            var entityType = entity.ClrType;
            if (entityType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAggregateRoot<>)))
                yield return entityType;
        }
        */
    }
}
