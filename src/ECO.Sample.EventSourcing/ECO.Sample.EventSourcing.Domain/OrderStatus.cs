namespace ECO.Sample.EventSourcing.Domain;

public enum OrderStatus
{
    NotSet = -1,
    New = 0,
    Prepared = 1,
    Shipped = 2
}
