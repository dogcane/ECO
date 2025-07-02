namespace ECO.EventSourcing;

/// <summary>
/// Represents an aggregate root with event sourcing capabilities.
/// Provides mechanisms for applying and tracking domain events, maintaining versioning, and managing uncommitted events.
/// </summary>
/// <typeparam name="T">The type of the aggregate root's identity.</typeparam>
public abstract class ESAggregateRoot<T> : AggregateRoot<T>, IESAggregateRoot<T>
{
    #region Fields

    /// <summary>
    /// Stores the list of uncommitted domain events.
    /// </summary>
    private readonly List<object> _uncommittedEvents = [];

    #endregion

    #region Properties

    /// <summary>
    /// Gets the current version of the aggregate root.
    /// </summary>
    public long Version { get; protected set; }

    #endregion

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="ESAggregateRoot{T}"/> class.
    /// </summary>
    protected ESAggregateRoot() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ESAggregateRoot{T}"/> class with the specified identity.
    /// </summary>
    /// <param name="identity">The identity of the aggregate root.</param>
    protected ESAggregateRoot(T identity) : base(identity) { }

    #endregion

    #region Protected_Methods

    /// <summary>
    /// Applies the specified event to the aggregate root using the provided handler, tracks it as uncommitted, and increments the version.
    /// </summary>
    /// <typeparam name="E">The type of the event.</typeparam>
    /// <param name="@event">The event instance to apply.</param>
    /// <param name="applyHandler">The handler that applies the event to the aggregate root.</param>
    protected void OnApply<E>(E @event, Action<E> applyHandler)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(applyHandler);
        applyHandler(@event);
        _uncommittedEvents.Add(@event);
        Version++;
    }

    #endregion

    #region IESAggregateRoot Methods

    /// <inheritdoc/>
    void IESAggregateRoot<T>.ClearUncommittedEvents() => _uncommittedEvents.Clear();

    /// <inheritdoc/>
    IEnumerable<object> IESAggregateRoot<T>.GetUncommittedEvents() => _uncommittedEvents.ToArray();

    #endregion
}
