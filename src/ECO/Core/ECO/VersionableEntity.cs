namespace ECO;

/// <summary>
/// Abstract base class for all versionable entities.
/// </summary>
/// <typeparam name="T">The type of the identifier.</typeparam>
public abstract class VersionableEntity<T> : Entity<T>, IVersionableEntity<T>
{
    #region Public_Properties

    /// <summary>
    /// Gets the version of the entity.
    /// </summary>
    public virtual int Version { get; protected set; }

    #endregion

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionableEntity{T}"/> class with default version 1.
    /// </summary>
    protected VersionableEntity() : base() => Version = 1;

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionableEntity{T}"/> class with a specific identifier and version.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <param name="version">The version of the entity.</param>
    protected VersionableEntity(T id, int version) : base(id) => Version = version;

    #endregion
}
