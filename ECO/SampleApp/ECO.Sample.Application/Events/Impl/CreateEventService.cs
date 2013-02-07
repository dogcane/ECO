using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Bender;

using ECO.Sample.Domain;

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

        public OperationResult CreateNewEvent(string name, string description, DateTime startDate, DateTime endDate)
        {
            return Event
                .Create(name, description, new Period(startDate, endDate))
                .IfSuccess<Event>(@event =>
                {
                    _EventRepository.Add(@event);
                })
                .IfFailed<Event>(result =>
                {
                        result.TranslateContext("Period.StartDate", "StartDate");
                });
        }

        #endregion
    }
}
