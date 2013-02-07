using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Linq;
using ECO.Bender;

using ECO.Sample.Domain;

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
            Session session = @event.Sessions.GetByIdentity(sessionCode);
            return @event.RemoveSession(session);
        }

        #endregion
    }
}
