using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Bender;

namespace ECO.Sample.Application.Events
{
    public interface IChangeEventService
    {
        OperationResult ChangeInformation(Guid eventCode, string name, string description);

        OperationResult ChangeDates(Guid eventCode, DateTime startDate, DateTime endDate);
    }
}
