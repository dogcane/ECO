namespace ECO;

/// <summary>
/// Abstract base class for all entities, providing identity and equality logic.
/// </summary>
/// <typeparam name="T">Type of the entity's identifier.</typeparam>
public abstract class Entity<T>(T? identity = default) : IEntity<T>
{
    #region Public_Properties

    /// <summary>
    /// Gets the identifier of the entity.
    /// </summary>
    public virtual T? Identity { get; protected set; } = identity;

    #endregion

    #region Public_Methods

    /// <summary>
    /// Determines whether the specified object is equal to the current entity.
    /// </summary>
    /// <param name="obj">The object to compare with the current entity.</param>
    /// <returns><c>true</c> if the specified object is equal to the current entity; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
        => obj is IEntity<T> entity && Equals(entity);

    /// <summary>
    /// Determines whether the specified entity is equal to the current entity.
    /// </summary>
    /// <param name="obj">The entity to compare with the current entity.</param>
    /// <returns><c>true</c> if the specified entity is equal to the current entity; otherwise, <c>false</c>.</returns>
    public virtual bool Equals(IEntity<T>? obj)
        => obj is not null && GetType() == obj.GetType() && EqualityComparer<T?>.Default.Equals(Identity, obj.Identity);

    /// <summary>
    /// Returns a hash code for the current entity.
    /// </summary>
    /// <returns>A hash code for the current entity.</returns>
    public override int GetHashCode() => HashCode.Combine(Identity, GetType());

    #endregion
}
