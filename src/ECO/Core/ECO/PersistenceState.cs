namespace ECO
{
    /// <summary>
    /// Enum that defines the state of an entity
    /// </summary>
    public enum PersistenceState
    {
        /// <summary>
        /// Unknown state
        /// </summary>
        Unknown,
        /// <summary>
        /// New entity
        /// </summary>
        Transient,
        /// <summary>
        /// Saved entity
        /// </summary>
        Persistent,
        /// <summary>
        /// Saved entity with no persistence context
        /// </summary>
        Detached
    }
}