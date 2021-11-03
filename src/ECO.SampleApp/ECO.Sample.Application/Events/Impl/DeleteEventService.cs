using ECO.Sample.Domain;
using Resulz;
using System;

namespace ECO.Sample.Application.Events.Impl
{
    public class DeleteEventService : IDeleteEventService
    {
        #region Private_Fields

        private IEventRepository _EventRepository;

        #endregion

        #region Ctor

        public DeleteEventService(IEventRepository eventRepository)
        {
            _EventRepository = eventRepository;
        }

        #endregion

        #region IDeleteEventService Membri di

        public OperationResult DeleteEvent(Guid eventCode)
        {
            Event @event = _EventRepository.Load(eventCode);
            _EventRepository.Remove(@event);
            return OperationResult.MakeSuccess();
        }

        #endregion
    }
}
