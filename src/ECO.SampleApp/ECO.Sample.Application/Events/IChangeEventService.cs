using ECO.Sample.Application.Events.DTO;
using Resulz;

namespace ECO.Sample.Application.Events
{
    public interface IChangeEventService
    {
        OperationResult ChangeInformation(EventDetail @event);
    }
}
