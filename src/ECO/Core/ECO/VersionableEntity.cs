namespace ECO;

/// <summary>
/// Abstract base class for all versionable entities.
/// </summary>
/// <typeparam name="T">The type of the identifier.</typeparam>
public abstract class VersionableEntity<T>(T? id = default, int version = 1) : Entity<T>(id), IVersionableEntity<T>
{
    #region Public_Properties

    /// <summary>
    /// Gets the version of the entity.
    /// </summary>
    public virtual int Version { get; protected set; } = version;

    #endregion
}
