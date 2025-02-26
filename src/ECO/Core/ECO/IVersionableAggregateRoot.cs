namespace ECO;

/// <summary>
/// Interface that defines a versionble Aggregate root
/// </summary>
/// <typeparam name="T">The type of the identifier</typeparam>
public interface IVersionableAggregateRoot<T> : IAggregateRoot<T>, IVersionableEntity<T>
{

}
