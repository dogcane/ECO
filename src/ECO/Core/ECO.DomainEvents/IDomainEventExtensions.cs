namespace ECO.Events;

/// <summary>
/// Provides extension methods for domain event management, enabling a fluent interface for raising events.
/// </summary>
public static class IDomainEventExtensions
{
    /// <summary>
    /// Raises the specified domain event using the <see cref="DomainEvents"/> infrastructure.
    /// </summary>
    /// <typeparam name="T">The type of the domain event, which must implement <see cref="IDomainEvent"/>.</typeparam>
    /// <param name="domainEvent">The domain event instance to raise.</param>
    public static void Raise<T>(this T domainEvent) where T : IDomainEvent
        => DomainEvents.RaiseEvent(domainEvent);
}
