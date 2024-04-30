namespace ECO.Events;

/// <summary>
/// Represents the extension for a fluent interface in domain events management
/// </summary>
public static class IDomainEventExtensions
{
    public static void Raise<T>(this T domainEvent) where T : IDomainEvent => DomainEvents.RaiseEvent(domainEvent);
}
