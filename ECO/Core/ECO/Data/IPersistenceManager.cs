namespace ECO.Data
{
    /// <summary>
    /// Interface that representa a generic persistence manager for an aggregate root and contains the link to the
    /// related persistence unit
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public interface IPersistenceManager<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Properties

        /// <summary>
        /// Related persistence unit
        /// </summary>
        IPersistenceUnit PersistenceUnit { get; }

        #endregion
    }
}
