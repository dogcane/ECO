namespace ECO;

/// <summary>
/// Class that defines a base for all the entities
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Entity<T> : IEntity<T>
{
    #region Public_Properties

    /// <summary>
    /// Identifier of the entity
    /// </summary>
    public virtual T? Identity { get; protected set; }

    #endregion

    #region Ctor

    /// <summary>
    /// Ctor
    /// </summary>
    protected Entity() => Identity = default;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="identity"></param>
    protected Entity(T identity) => Identity = identity;

    #endregion

    #region Public_Methods

    /// <summary>
    /// Method that verify if another object is the same as the current entity
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals(obj as IEntity<T>);

    /// <summary>
    /// Method that verify if another entitye is the same as the current entity
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public virtual bool Equals(IEntity<T>? obj) => (obj != null) && GetType() == obj!.GetType() && object.Equals(Identity, obj!.Identity);

    /// <summary>
    /// Method that returns the hash code for the current entity
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Identity, GetType());

    #endregion
}
