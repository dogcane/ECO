using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Linq;

using ECO.Sample.Domain;
using ECO.Sample.Application.Events.DTO;
using ECO.Sample.Application.DTO;

namespace ECO.Sample.Application.Events.Impl
{
    public class ShowEventsService : IShowEventsService
    {
        #region Private_Fields

        private IEventRepository _EventRepository;

        #endregion

        #region Ctor

        public ShowEventsService(IEventRepository eventRepository)
        {
            _EventRepository = eventRepository;
        }

        #endregion

        #region IShowEventsService Membri di

        public PageableList<EventListItem> ShowEvents(DateTime? fromDate, DateTime? toDate, int page, int pageSize)
        {
            var query = _EventRepository.AsQueryable();
            if (fromDate.HasValue)
            {
                query = query.Where(entity => entity.Period.StartDate >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                query = query.Where(entity => entity.Period.EndDate <= toDate.Value);
            }
            return new PageableList<EventListItem>()
            {
                CurrentElements = query.Paged(page, pageSize).Select(item => EventListItem.From(item)),
                PageSize = pageSize,
                CurrentPage = page,
                TotalElements = query.Count()
            };
        }

        #endregion
    }
}
