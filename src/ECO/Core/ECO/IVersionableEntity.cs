namespace ECO;

/// <summary>
/// Interface that defines a versionble Entity
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IVersionableEntity<T> : IEntity<T>
{
    #region Properties

    /// <summary>
    /// Version of the entity
    /// </summary>
    int Version { get; }

    #endregion
}
