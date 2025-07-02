namespace ECO;

/// <summary>
/// Interface that defines an entity with an identifier of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the identifier.</typeparam>
public interface IEntity<T> : IEquatable<IEntity<T>>
{
    #region Properties

    /// <summary>
    /// Gets the identifier of the entity.
    /// </summary>
    T? Identity { get; }

    #endregion
}
