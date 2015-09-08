using System;
using System.Linq;
using System.Web.Http;

using ECO.Integrations.Web.MVC;

using ECO.Sample.Application.Events;
using ECO.Sample.Application.Events.DTO;
using ECO.Bender;
using System.Net;

namespace ECO.Sample.Presentation.Areas.Events.Controllers
{
    [RoutePrefix("events/api")]
    public class EventApiController : ApiController
    {
        private IShowEventsService _ShowEventsService;

        private ICreateEventService _CreateEventService;

        private IGetEventService _ShowEventDetailService;

        private IChangeEventService _ChangeEventService;

        private IDeleteEventService _DeleteEventService;

        private IAddSessionToEventService _AddSessionToEventService;

        private IRemoveSessionFromEventService _RemoveSessionFromEventService;

        public EventApiController(IShowEventsService showEventService, ICreateEventService createEventService,
            IGetEventService showEventDetailService, IChangeEventService changeEventService, IDeleteEventService deleteEventService,
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
        public OperationResult<EventDetail> GetEventById(Guid eventCode)
        {
            var result = _ShowEventDetailService.GetEvent(eventCode);
            if (result.Success)
            {
                return result;
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        [DataContextApiFilter]
        [HttpPost]
        [Route()]
        public OperationResult<Guid> CreateEvent([FromBody]EventDetail @event)
        {
            return _CreateEventService.CreateNewEvent(@event);
        }

        [DataContextApiFilter]
        [HttpPut]
        [Route()]
        public OperationResult UpdateEvent([FromBody]EventDetail @event)
        {
            var result = _ShowEventDetailService.GetEvent(@event.EventCode);
            if (!result.Success)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return _ChangeEventService.ChangeInformation(@event);
        }

        [DataContextApiFilter]
        [HttpDelete]
        [Route("{id:guid}")]
        public void DeleteEvent(Guid eventCode)
        {
        }
    }
}
