namespace ECO.Providers.EntityFramework.Utils;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Utility class for working with DbContext types and extracting aggregate root information.
/// </summary>
/// <typeparam name="T">The DbContext type to work with.</typeparam>
internal static class DbContextFacade<T>
    where T : DbContext
{
    #region Public_Methods

    /// <summary>
    /// Gets all aggregate root types from the specified DbContext type by examining its DbSet properties.
    /// </summary>
    /// <returns>An enumerable of aggregate root types found in the DbContext.</returns>
    public static IEnumerable<Type> GetAggregateTypesFromDBContext()
        => typeof(T)
            .GetProperties()
            .Where(IsDbSetProperty)
            .Select(prop => prop.PropertyType.GetGenericArguments()[0])
            .Where(IsAggregateRootType);

    #endregion

    #region Private_Methods

    /// <summary>
    /// Determines if a property represents a DbSet property.
    /// </summary>
    /// <param name="property">The property to check.</param>
    /// <returns>True if the property is a DbSet property, false otherwise.</returns>
    private static bool IsDbSetProperty(System.Reflection.PropertyInfo property) =>
        property.PropertyType.IsGenericType &&
        property.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>);

    /// <summary>
    /// Determines if a type implements IAggregateRoot interface.
    /// </summary>
    /// <param name="entityType">The type to check.</param>
    /// <returns>True if the type implements IAggregateRoot, false otherwise.</returns>
    private static bool IsAggregateRootType(Type entityType) =>
        entityType.GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAggregateRoot<>));

    #endregion
}
