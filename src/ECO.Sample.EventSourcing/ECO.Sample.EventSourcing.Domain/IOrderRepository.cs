using ECO.EventSourcing;

namespace ECO.Sample.EventSourcing.Domain;

public interface IOrderRepository : IESRepository<Order, string>
{

}
