using ECO.Sample.Domain;
using Resulz;
using System;
using System.Linq;

namespace ECO.Sample.Application.Events.Impl
{
    public class RemoveSessionFromEventService : IRemoveSessionFromEventService
    {
        #region Private_Fields

        private IEventRepository _EventRepository;

        #endregion

        #region Ctor

        public RemoveSessionFromEventService(IEventRepository eventRepository)
        {
            _EventRepository = eventRepository;
        }

        #endregion

        #region IRemoveSessionFromEventService Membri di

        public OperationResult RemoveSession(Guid eventCode, Guid sessionCode)
        {
            Event @event = _EventRepository.Load(eventCode);
            Session session = @event.Sessions.FirstOrDefault(s => s.Identity == sessionCode);
            return @event.RemoveSession(session);
        }

        #endregion
    }
}
