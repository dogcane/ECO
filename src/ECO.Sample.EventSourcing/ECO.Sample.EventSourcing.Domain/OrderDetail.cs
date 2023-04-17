namespace ECO.Sample.EventSourcing.Domain;

public class OrderDetail
{
    public int Sku { get; set; }

    public string? Description { get; set; }

    public int Quantity { get; set; }

    public float Amount { get; set; }

    public float TotalAmount => Amount * Quantity;

    public override string ToString() => $"Item : {Sku} - Quantity : {Quantity} - TotalAmount : {TotalAmount}";
}
