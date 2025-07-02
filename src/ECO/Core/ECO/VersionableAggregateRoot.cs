namespace ECO;

/// <summary>
/// Abstract base class for all versionable aggregate roots.
/// </summary>
/// <typeparam name="T">The type of the identifier.</typeparam>
public abstract class VersionableAggregateRoot<T> : AggregateRoot<T>, IVersionableAggregateRoot<T>
{
    #region Public_Properties

    /// <summary>
    /// Gets the version of the aggregate root.
    /// </summary>
    public virtual int Version { get; protected set; }

    #endregion

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionableAggregateRoot{T}"/> class with default version 1.
    /// </summary>
    protected VersionableAggregateRoot() : base() => Version = 1;

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionableAggregateRoot{T}"/> class with a specific identifier and version.
    /// </summary>
    /// <param name="id">The identifier of the aggregate root.</param>
    /// <param name="version">The version of the aggregate root.</param>
    protected VersionableAggregateRoot(T id, int version) : base(id) => Version = version;

    #endregion
}
