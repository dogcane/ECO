namespace ECO.Data;

/// <summary>
/// Represents a generic persistence manager for an aggregate root, providing access to the related persistence context.
/// </summary>
/// <typeparam name="T">The type of the aggregate root.</typeparam>
/// <typeparam name="K">The type of the aggregate root's identifier.</typeparam>
public interface IPersistenceManager<T, K>
    where T : class, IAggregateRoot<K>
    where K : notnull, IEquatable<K>
{
    #region Properties

    /// <summary>
    /// Gets the related persistence context for the aggregate root.
    /// </summary>
    IPersistenceContext PersistenceContext { get; }

    #endregion
}
