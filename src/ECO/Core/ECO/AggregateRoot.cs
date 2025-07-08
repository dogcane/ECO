namespace ECO;

/// <summary>
/// Abstract base class for all aggregate roots, inheriting from Entity and implementing IAggregateRoot.
/// </summary>
/// <typeparam name="T">Type of the aggregate root's identifier.</typeparam>
public abstract class AggregateRoot<T>(T? identity = default) : Entity<T>(identity), IAggregateRoot<T>
{
    
}
