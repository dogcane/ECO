namespace ECO.Events;

/// <summary>
/// Represents a delegate for a event's callback
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="sourceEvent"></param>
public delegate void EventAction<T>(T sourceEvent);
