namespace ECO.Providers.EntityFramework.Utils;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

internal static class DbContextFacade<T>
    where T : DbContext
{
    #region AggregateTypeHelpers
    public static IEnumerable<Type> GetAggregateTypesFromDBContext() =>
        [.. typeof(T)
            .GetProperties()
            .Where(prop => prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
            .Select(prop => prop.PropertyType.GetGenericArguments()[0])
            .Where(entityType => entityType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAggregateRoot<>)))];
    #endregion
}
