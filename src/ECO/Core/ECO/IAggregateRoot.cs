namespace ECO;

/// <summary>
/// Interface that defines an entity who is the root of its aggregate
/// </summary>
/// <typeparam name="T">The type of the identifier</typeparam>
public interface IAggregateRoot<T> : IEntity<T>
{

}
