using System;
using System.Linq;
using System.Web.Http;

using ECO.Web.MVC;

using ECO.Sample.Application.Events;
using ECO.Sample.Application.Events.DTO;
using ECO.Bender;

namespace ECO.Sample.Presentation.Areas.Events.Controllers
{
    [RoutePrefix("events/api")]
    public class EventApiController : ApiController
    {
        private IShowEventsService _ShowEventsService;

        private ICreateEventService _CreateEventService;

        private IShowEventDetailService _ShowEventDetailService;

        private IChangeEventService _ChangeEventService;

        private IDeleteEventService _DeleteEventService;

        private IAddSessionToEventService _AddSessionToEventService;

        private IRemoveSessionFromEventService _RemoveSessionFromEventService;

        public EventApiController(IShowEventsService showEventService, ICreateEventService createEventService,
            IShowEventDetailService showEventDetailService, IChangeEventService changeEventService, IDeleteEventService deleteEventService,
            IAddSessionToEventService addSessionToEventService, IRemoveSessionFromEventService removeSessionFromEventService)
        {
            _ShowEventsService = showEventService;
            _CreateEventService = createEventService;
            _ShowEventDetailService = showEventDetailService;
            _ChangeEventService = changeEventService;
            _DeleteEventService = deleteEventService;
            _AddSessionToEventService = addSessionToEventService;
            _RemoveSessionFromEventService = removeSessionFromEventService;
        }

        [DataContextApiFilter]
        [HttpGet]
        [Route()]
        public IQueryable<EventListItem> GetEvents(DateTime? start, DateTime? end, string eventName)
        {
            return _ShowEventsService.ShowEvents(start, end, eventName);
        }

        [DataContextApiFilter]
        [HttpGet]
        [Route("{id:guid}")]
        public EventDetail GetEventById(Guid eventCode)
        {
            return _ShowEventDetailService.ShowDetail(eventCode);
        }

        [DataContextApiFilter]
        [HttpPost]
        [Route()]
        public OperationResult<Guid> CreateEvent([FromBody]EventDetail @event)
        {
            return _CreateEventService.CreateNewEvent(@event);
        }

        // PUT api/event/5
        public void Put(Guid eventCode, [FromBody]string name, [FromBody]string description, [FromBody]DateTime startDate, [FromBody]DateTime endDate)
        {
            
        }

        // DELETE api/event/5
        public void Delete(Guid eventCode)
        {
        }
    }
}
