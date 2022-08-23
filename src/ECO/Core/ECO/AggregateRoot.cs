namespace ECO
{
    /// <summary>
    /// Class that defines a base for all aggregates roots
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot<T>
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        protected AggregateRoot() : base() { }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="identity"></param>
        protected AggregateRoot(T identity) : base(identity) { }

        #endregion
    }
}
