using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Bender;

namespace ECO.Sample.Application.Events
{
    public interface ICreateEventService
    {
        OperationResult CreateNewEvent(string name, string description, DateTime startDate, DateTime endDate);
    }
}
