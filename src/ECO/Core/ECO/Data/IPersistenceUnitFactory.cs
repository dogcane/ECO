namespace ECO.Data;

/// <summary>
/// Defines a factory for managing and retrieving persistence units and opening data contexts.
/// </summary>
public interface IPersistenceUnitFactory
{
    /// <summary>
    /// Adds a persistence unit to the factory.
    /// </summary>
    /// <param name="persistenceUnit">The persistence unit to add.</param>
    /// <returns>The factory instance for chaining.</returns>
    IPersistenceUnitFactory AddPersistenceUnit(IPersistenceUnit persistenceUnit);

    /// <summary>
    /// Gets a persistence unit by its name.
    /// </summary>
    /// <param name="name">The name of the persistence unit.</param>
    /// <returns>The persistence unit instance.</returns>
    IPersistenceUnit GetPersistenceUnit(string name);

    /// <summary>
    /// Gets a persistence unit for the specified entity type.
    /// </summary>
    /// <param name="entityType">The entity type.</param>
    /// <returns>The persistence unit instance.</returns>
    IPersistenceUnit GetPersistenceUnit(Type entityType);

    /// <summary>
    /// Gets a persistence unit for the specified entity type.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <returns>The persistence unit instance.</returns>
    IPersistenceUnit GetPersistenceUnit<T>();

    /// <summary>
    /// Opens a new data context.
    /// </summary>
    /// <returns>The opened data context.</returns>
    IDataContext OpenDataContext();
}