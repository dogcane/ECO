using Resulz;
using System;

namespace ECO.Sample.Application.Events
{
    public interface IRemoveSessionFromEventService
    {
        OperationResult RemoveSession(Guid eventCode, Guid sessionCode);
    }
}
