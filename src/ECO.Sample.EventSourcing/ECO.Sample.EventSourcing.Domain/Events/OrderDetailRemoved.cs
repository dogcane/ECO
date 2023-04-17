namespace ECO.Sample.EventSourcing.Events;

public record OrderDetailRemoved(string Id, int Sku, int Quantity)
{
    public override string ToString() => $"Order {Id} detail removed : {Sku} - Qty:{Quantity}";
}