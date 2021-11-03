using ECO.Sample.Application.Events.DTO;
using Resulz;
using System;

namespace ECO.Sample.Application.Events
{
    public interface ICreateEventService
    {
        OperationResult<Guid> CreateNewEvent(EventDetail @event);
    }
}
