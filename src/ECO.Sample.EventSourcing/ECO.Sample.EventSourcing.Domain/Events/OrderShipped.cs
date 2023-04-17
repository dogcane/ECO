namespace ECO.Sample.EventSourcing.Events;
public record OrderShipped(string Id)
{ 
    public override string ToString() => $"Order {Id} shipped";
}
