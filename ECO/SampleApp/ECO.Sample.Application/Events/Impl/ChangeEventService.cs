using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECO.Bender;
using ECO.Sample.Application.Events.DTO;
using ECO.Sample.Domain;

namespace ECO.Sample.Application.Events.Impl
{
    public class ChangeEventService : IChangeEventService
    {
        #region Private_Fields

        private IEventRepository _EventRepository;

        #endregion

        #region Ctor

        public ChangeEventService(IEventRepository eventRepository)
        {
            _EventRepository = eventRepository;
        }

        #endregion

        #region IChangeEventService Membri di

        public OperationResult ChangeInformation(EventDetail @event)
        {
            Event eventEventity = _EventRepository.Load(@event.EventCode);
            return eventEventity
                .ChangeInformation(@event.Name, @event.Description, new Period(@event.StartDate, @event.EndDate))
                .IfSuccess(() => _EventRepository.Update(eventEventity));
        }

        #endregion
    }
}
