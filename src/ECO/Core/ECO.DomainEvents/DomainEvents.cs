namespace ECO.Events;

using System.Collections.Concurrent;

/// <summary>
/// Provides a static manager for domain events, allowing subscription, unsubscription, and event raising.
/// </summary>
public static class DomainEvents
{
    #region Private_Fields

    // Stores event subscribers by event type and subscriber key.
    private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<object, Delegate>> _Subscribers = new();

    #endregion

    #region Events

    /// <summary>
    /// Occurs when any domain event is raised.
    /// </summary>
    public static event EventHandler<DomainEventArgs<IDomainEvent>>? EventRaised;

    /// <summary>
    /// Occurs when a managed domain event is invoked for a subscriber.
    /// </summary>
    public static event EventHandler<DomainEventArgs<IDomainEvent, Delegate>>? EventManaged;

    #endregion

    #region Public_Methods

    /// <summary>
    /// Raises a domain event, invoking all registered subscribers for the event type.
    /// </summary>
    /// <typeparam name="T">The type of the domain event.</typeparam>
    /// <param name="args">The event arguments.</param>
    public static void RaiseEvent<T>(T args) where T : IDomainEvent
    {
        EventRaised?.Invoke(null, new DomainEventArgs<IDomainEvent>(args));
        if (_Subscribers.TryGetValue(typeof(T), out var subscribers))
        {
            foreach (var act in subscribers.Values)
            {
                if (act is EventAction<T> action)
                {
                    EventManaged?.Invoke(null, new DomainEventArgs<IDomainEvent, Delegate>(args, action));
                    action(args);
                }
            }
        }
    }

    /// <summary>
    /// Subscribes a callback to a domain event for a specific subscriber.
    /// </summary>
    /// <typeparam name="T">The type of the domain event.</typeparam>
    /// <param name="subscriber">The subscriber key (usually 'this').</param>
    /// <param name="callback">The callback to invoke when the event is raised.</param>
    public static void Subscribe<T>(object subscriber, EventAction<T> callback) where T : IDomainEvent
    {
        ArgumentNullException.ThrowIfNull(subscriber);
        ArgumentNullException.ThrowIfNull(callback);
        var type = typeof(T);
        var subscribers = _Subscribers.GetOrAdd(type, _ => new ConcurrentDictionary<object, Delegate>());
        subscribers[subscriber] = callback;
    }

    /// <summary>
    /// Unsubscribes a callback from a domain event for a specific subscriber.
    /// </summary>
    /// <typeparam name="T">The type of the domain event.</typeparam>
    /// <param name="subscriber">The subscriber key (usually 'this').</param>
    public static void Unsubscribe<T>(object subscriber) where T : IDomainEvent
    {
        ArgumentNullException.ThrowIfNull(subscriber);
        var type = typeof(T);
        if (_Subscribers.TryGetValue(type, out var subscribers))
        {
            subscribers.TryRemove(subscriber, out _);
        }
    }

    #endregion
}
