namespace ECO.EventSourcing;

using ECO.Data;

/// <summary>
/// Defines a contract for a repository that supports event sourcing operations for aggregates.
/// Provides methods for loading, saving, and retrieving events for aggregates.
/// </summary>
/// <typeparam name="T">The type of the aggregate root, which must implement <see cref="IESAggregateRoot{K}"/>.</typeparam>
/// <typeparam name="K">The type of the aggregate root's identity.</typeparam>
public interface IESRepository<T, K> : IPersistenceManager<T, K>
    where T : class, IESAggregateRoot<K>
{
    /// <summary>
    /// Loads an aggregate by its identity.
    /// </summary>
    /// <param name="identity">The identity of the aggregate.</param>
    /// <returns>The loaded aggregate, or <c>null</c> if not found.</returns>
    T? Load(K identity);

    /// <summary>
    /// Asynchronously loads an aggregate by its identity.
    /// </summary>
    /// <param name="identity">The identity of the aggregate.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the loaded aggregate, or <c>null</c> if not found.</returns>
    ValueTask<T?> LoadAsync(K identity);

    /// <summary>
    /// Saves the specified aggregate.
    /// </summary>
    /// <param name="item">The aggregate to save.</param>
    void Save(T item);

    /// <summary>
    /// Asynchronously saves the specified aggregate.
    /// </summary>
    /// <param name="item">The aggregate to save.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task SaveAsync(T item);

    /// <summary>
    /// Loads the events for the specified aggregate identity.
    /// </summary>
    /// <param name="identity">The identity of the aggregate.</param>
    /// <returns>An <see cref="IEnumerable{dynamic}"/> containing the events for the aggregate.</returns>
    IEnumerable<dynamic> LoadEvents(K identity);

    /// <summary>
    /// Asynchronously loads the events for the specified aggregate identity.
    /// </summary>
    /// <param name="identity">The identity of the aggregate.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the events for the aggregate.</returns>
    Task<IEnumerable<dynamic>> LoadEventsAsync(K identity);
}
