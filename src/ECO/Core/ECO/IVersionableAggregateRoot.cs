namespace ECO;

/// <summary>
/// Interface that defines a versionble Aggregate root
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IVersionableAggregateRoot<T> : IAggregateRoot<T>, IVersionableEntity<T>
{

}
