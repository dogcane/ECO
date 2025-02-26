namespace ECO;

/// <summary>
/// Class that defines a base for all versionable entities
/// </summary>
/// <typeparam name="T">The type of the identifier</typeparam>
public abstract class VersionableEntity<T> : Entity<T>, IVersionableEntity<T>
{
    #region Public_Properties

    /// <summary>
    /// Version of the entity
    /// </summary>
    public virtual int Version { get; protected set; }

    #endregion

    #region Ctor

    /// <summary>
    /// Ctor
    /// </summary>
    protected VersionableEntity() : base() => Version = 1;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="id"></param>
    /// <param name="version"></param>
    protected VersionableEntity(T id, int version) : base(id) => Version = version;

    #endregion
}
