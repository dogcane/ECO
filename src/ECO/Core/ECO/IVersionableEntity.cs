namespace ECO;

/// <summary>
/// Interface that defines a versionable entity.
/// </summary>
/// <typeparam name="T">The type of the identifier.</typeparam>
public interface IVersionableEntity<T> : IEntity<T>
{
    #region Properties

    /// <summary>
    /// Gets the version of the entity.
    /// </summary>
    int Version { get; }

    #endregion
}
