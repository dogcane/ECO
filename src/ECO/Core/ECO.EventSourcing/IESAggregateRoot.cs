namespace ECO.EventSourcing;

/// <summary>
/// Defines a contract for an aggregate root that supports event sourcing.
/// Provides access to versioning and management of uncommitted domain events.
/// </summary>
/// <typeparam name="T">The type of the aggregate root's identity.</typeparam>
public interface IESAggregateRoot<T> : IAggregateRoot<T>
{
    /// <summary>
    /// Gets the current version of the aggregate root.
    /// </summary>
    long Version { get; }

    /// <summary>
    /// Retrieves the collection of uncommitted domain events.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{Object}"/> containing all uncommitted events.</returns>
    IEnumerable<object> GetUncommittedEvents();

    /// <summary>
    /// Clears all uncommitted domain events from the aggregate root.
    /// </summary>
    void ClearUncommittedEvents();
}
