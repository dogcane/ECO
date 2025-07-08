namespace ECO.Events;

/// <summary>
/// Represents a delegate for handling a domain event callback with a strongly-typed event argument.
/// </summary>
/// <typeparam name="T">
/// The type of the event argument, typically implementing <see cref="IDomainEvent"/>.
/// </typeparam>
/// <param name="sourceEvent">
/// The event instance that triggered the callback.
/// </param>
public delegate void EventAction<T>(T sourceEvent) where T : IDomainEvent;
