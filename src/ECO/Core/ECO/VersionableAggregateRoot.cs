namespace ECO;

/// <summary>
/// Abstract base class for all versionable aggregate roots.
/// </summary>
/// <typeparam name="T">The type of the identifier.</typeparam>
public abstract class VersionableAggregateRoot<T>(T? id = default, int version = 1) : AggregateRoot<T>(id), IVersionableAggregateRoot<T>
{
    #region Public_Properties

    /// <summary>
    /// Gets the version of the aggregate root.
    /// </summary>
    public virtual int Version { get; protected set; } = version;

    #endregion
}
