namespace ECO.EventSourcing;

public interface IESAggregateRoot<T> : IAggregateRoot<T>
{
    long Version { get; }

    IEnumerable<object> GetUncommittedEvents();

    void ClearUncommittedEvents();
}
