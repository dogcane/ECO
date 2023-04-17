namespace ECO.Sample.EventSourcing.Events;

public record OrderPrepared(string Id)
{
    public override string ToString() => $"Order {Id} prepared for shipping";
}
