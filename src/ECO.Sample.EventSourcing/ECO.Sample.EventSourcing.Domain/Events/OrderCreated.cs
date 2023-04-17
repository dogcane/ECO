namespace ECO.Sample.EventSourcing.Events;

public record OrderCreated(string Id, string Name, string Surname, string Address)
{
    public override string ToString() => $"Order {Id} created for {Name} {Surname}";
}

