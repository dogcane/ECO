using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;

using ECO.Sample.Domain;
using ECO.Sample.Application.Events.DTO;

namespace ECO.Sample.Application.Events.Impl
{
    public class ShowEventDetailService : IShowEventDetailService
    {
        #region Private_Fields

        private IEventRepository _EventRepository;

        #endregion

        #region Ctor

        public ShowEventDetailService(IEventRepository eventRepository)
        {
            _EventRepository = eventRepository;
        }

        #endregion

        #region IShowEventDetailService Membri di

        public EventDetail ShowDetail(Guid eventCode)
        {
            Event @event = _EventRepository.Load(eventCode);
            return (@event != null) ? EventDetail.From(@event) : EventDetail.Empty;
        }

        #endregion
    }
}
