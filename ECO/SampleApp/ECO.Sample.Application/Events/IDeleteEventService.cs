using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Bender;

namespace ECO.Sample.Application.Events
{
    public interface IDeleteEventService
    {
        OperationResult DeleteEvent(Guid eventCode);
    }
}
