using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Bender;

using ECO.Sample.Domain;
using ECO.Sample.Application.Events.DTO;

namespace ECO.Sample.Application.Events.Impl
{
    public class CreateEventService : ICreateEventService
    {
        #region Private_Fields

        private IEventRepository _EventRepository;

        #endregion

        #region Ctor

        public CreateEventService(IEventRepository eventRepository)
        {
            _EventRepository = eventRepository;
        }

        #endregion

        #region IServizioCreaEvento Membri di

        public OperationResult<Guid> CreateNewEvent(EventDetail @event)
        {
            var eventResult = Event.Create(@event.Name, @event.Description, new Period(@event.StartDate, @event.EndDate));
            if (eventResult.Success)
            {
                _EventRepository.Add(eventResult.Value);
                return OperationResult<Guid>.MakeSuccess(eventResult.Value.Identity);
            }
            else
            {
                return OperationResult<Guid>.MakeFailure(eventResult.Errors).TranslateContext("Period.StartDate", "StartDate");
            }
        }

        #endregion
    }
}
