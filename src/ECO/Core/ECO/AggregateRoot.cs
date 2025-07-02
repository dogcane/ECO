namespace ECO;

/// <summary>
/// Abstract base class for all aggregate roots, inheriting from Entity and implementing IAggregateRoot.
/// </summary>
/// <typeparam name="T">Type of the aggregate root's identifier.</typeparam>
public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot<T>
{
    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot{T}"/> class with the default identity.
    /// </summary>
    protected AggregateRoot() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot{T}"/> class with a specific identity.
    /// </summary>
    /// <param name="identity">The aggregate root identifier.</param>
    protected AggregateRoot(T identity) : base(identity) { }

    #endregion
}
