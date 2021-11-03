using Resulz;
using System;

namespace ECO.Sample.Application.Events
{
    public interface IDeleteEventService
    {
        OperationResult DeleteEvent(Guid eventCode);
    }
}
