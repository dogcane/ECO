using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Bender;
using ECO.Sample.Application.Events.DTO;

namespace ECO.Sample.Application.Events
{
    public interface IChangeEventService
    {
        OperationResult ChangeInformation(EventDetail @event);
    }
}
