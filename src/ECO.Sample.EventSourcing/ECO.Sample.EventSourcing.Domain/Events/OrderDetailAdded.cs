namespace ECO.Sample.EventSourcing.Events;

public record OrderDetailAdded(string Id, int Sku, string Description, int Quantity, float Amount)
{
    public override string ToString() => $"Order {Id} detail added : {Sku} - Qty:{Quantity}";
}