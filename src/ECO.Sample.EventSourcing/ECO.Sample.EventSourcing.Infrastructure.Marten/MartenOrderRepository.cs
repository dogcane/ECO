using ECO.Data;
using ECO.Providers.Marten;
using ECO.Sample.EventSourcing.Domain;

namespace ECO.Sample.EventSourcing.Infrastructure.Marten;

public class MartenOrderRepository(IDataContext dataContext) : MartenESRepository<Order, string>(dataContext)
{
}
