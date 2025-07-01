namespace ECO;

/// <summary>
/// Interface that defines a versionable aggregate root.
/// </summary>
/// <typeparam name="T">The type of the identifier.</typeparam>
public interface IVersionableAggregateRoot<T> : IAggregateRoot<T>, IVersionableEntity<T>
{
    // Marker interface for versionable aggregate roots.
}
