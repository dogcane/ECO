namespace ECO;

/// <summary>
/// Interface that defines an Entity with an identifier of T type
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IEntity<T> : IEquatable<IEntity<T>>
{
    #region Properties

    /// <summary>
    /// Identifier of the entity
    /// </summary>
    T? Identity { get; }

    #endregion
}
