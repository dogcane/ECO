using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;

using ECO.Sample.Domain;
using ECO.Sample.Application.Events.DTO;
using ECO.Bender;

namespace ECO.Sample.Application.Events.Impl
{
    public class GetEventService : IGetEventService
    {
        #region Private_Fields

        private IEventRepository _EventRepository;

        #endregion

        #region Ctor

        public GetEventService(IEventRepository eventRepository)
        {
            _EventRepository = eventRepository;
        }

        #endregion

        #region IShowEventDetailService Membri di

        public OperationResult<EventDetail> GetEvent(Guid eventCode)
        {
            Event @event = _EventRepository.Load(eventCode);
            if (@event != null)
            {
                return EventDetail.From(@event);
            }
            else
            {
                return OperationResult<EventDetail>.MakeFailure(Enumerable.Empty<ErrorMessage>());
            }
        }

        #endregion
    }
}
