using Microsoft.Extensions.Configuration;

namespace ECO.Data;

/// <summary>
/// Defines a persistence unit, which represents a storage backend (e.g., a relational database) and all the classes persisted in it.
/// Provides methods for managing classes, listeners, and creating repositories and contexts.
/// </summary>
public interface IPersistenceUnit
{
    #region Properties

    /// <summary>
    /// Gets the name of the persistence unit.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the collection of class types managed by this persistence unit.
    /// </summary>
    IEnumerable<Type> Classes { get; }

    #endregion

    #region Methods

    /// <summary>
    /// Initializes the persistence unit with the specified attributes and configuration.
    /// </summary>
    /// <param name="extendedAttributes">Additional attributes for configuration.</param>
    /// <param name="configuration">The application configuration.</param>
    void Initialize(IDictionary<string, string> extendedAttributes, IConfiguration configuration);

    /// <summary>
    /// Returns the current persistence context for the opened data context, if there is one.
    /// </summary>
    /// <returns>The current persistence context.</returns>
    /// <exception cref="ApplicationException">Thrown if there is no active data context.</exception>
    IPersistenceContext CreateContext();

    /// <summary>
    /// Adds a class type to the current persistence unit.
    /// </summary>
    /// <param name="classType">The class type to add.</param>
    /// <returns>The persistence unit instance.</returns>
    IPersistenceUnit AddClass(Type classType);

    /// <summary>
    /// Adds a class type to the current persistence unit.
    /// </summary>
    /// <typeparam name="T">The aggregate root type.</typeparam>
    /// <typeparam name="K">The identifier type.</typeparam>
    /// <returns>The persistence unit instance.</returns>
    IPersistenceUnit AddClass<T, K>() where T : class, IAggregateRoot<K>;

    /// <summary>
    /// Removes a class type from the current persistence unit.
    /// </summary>
    /// <param name="classType">The class type to remove.</param>
    /// <returns>The persistence unit instance.</returns>
    IPersistenceUnit RemoveClass(Type classType);

    /// <summary>
    /// Removes a class type from the current persistence unit.
    /// </summary>
    /// <typeparam name="T">The aggregate root type.</typeparam>
    /// <typeparam name="K">The identifier type.</typeparam>
    /// <returns>The persistence unit instance.</returns>
    IPersistenceUnit RemoveClass<T, K>() where T : class, IAggregateRoot<K>;

    /// <summary>
    /// Adds a listener to the current persistence unit.
    /// </summary>
    /// <param name="listener">The listener to add.</param>
    /// <returns>The persistence unit instance.</returns>
    IPersistenceUnit AddUnitListener(IPersistenceUnitListener listener);

    /// <summary>
    /// Removes a listener from the current persistence unit.
    /// </summary>
    /// <param name="listener">The listener to remove.</param>
    /// <returns>The persistence unit instance.</returns>
    IPersistenceUnit RemoveUnitListener(IPersistenceUnitListener listener);

    /// <summary>
    /// Builds a read-only repository for the specified aggregate root type and identifier.
    /// </summary>
    /// <typeparam name="T">The aggregate root type.</typeparam>
    /// <typeparam name="K">The identifier type.</typeparam>
    /// <param name="dataContext">The data context to use.</param>
    /// <returns>The read-only repository instance.</returns>
    IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext)
        where T : class, IAggregateRoot<K>
        where K : notnull;

    /// <summary>
    /// Builds a repository for the specified aggregate root type and identifier.
    /// </summary>
    /// <typeparam name="T">The aggregate root type.</typeparam>
    /// <typeparam name="K">The identifier type.</typeparam>
    /// <param name="dataContext">The data context to use.</param>
    /// <returns>The repository instance.</returns>
    IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext)
        where T : class, IAggregateRoot<K>
        where K : notnull;

    #endregion
}
