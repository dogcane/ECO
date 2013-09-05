using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Bender;

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

        public OperationResult ChangeInformation(Guid eventCode, string name, string description, DateTime startDate, DateTime endDate)
        {
            Event @event = _EventRepository.Load(eventCode);
            return @event.ChangeInformation(name, description, new Period(startDate, endDate));
        }

        #endregion
    }
}
