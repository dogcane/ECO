using Resulz;
using System;

namespace ECO.Sample.Application.Events
{
    public interface IAddSessionToEventService
    {
        OperationResult AddSession(Guid eventCode, string title, string description, int level, Guid speakerCode);
    }
}
