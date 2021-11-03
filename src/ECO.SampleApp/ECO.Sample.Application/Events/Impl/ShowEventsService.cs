using ECO.Sample.Application.Events.DTO;
using ECO.Sample.Domain;
using System;
using System.Linq;

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

        public IQueryable<EventListItem> ShowEvents(DateTime? fromDate, DateTime? toDate, string eventName)
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
            if (!string.IsNullOrEmpty(eventName))
            {
                query = query.Where(entity => entity.Name.Contains(eventName));
            }
            return query.Select(item => new EventListItem
            {
                EventCode = item.Identity,
                Name = item.Name,
                StartDate = item.Period.StartDate,
                EndDate = item.Period.EndDate,
                NumberOfSessions = item.Sessions.Count()
            });
        }

        #endregion
    }
}
