namespace ECO;

/// <summary>
/// Enum that defines the state of an entity.
/// </summary>
public enum PersistenceState
{
    /// <summary>
    /// Unknown state.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// New entity.
    /// </summary>
    Transient = 1,

    /// <summary>
    /// Saved entity.
    /// </summary>
    Persistent = 2,

    /// <summary>
    /// Saved entity with no persistence context.
    /// </summary>
    Detached = 3
}