using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Bender;

namespace ECO.Sample.Application.Events
{
    public interface IAddSessionToEventService
    {
        OperationResult AddSession(Guid eventCode, string title, string description, int level, Guid speakerCode);
    }
}
