using ECO.Data;
using ECO.Sample.Application.DTO;
using ECO.Sample.Application.Events;
using ECO.Sample.Application.Events.DTO;
using ECO.Web.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ECO.Sample.Presentation.Areas.Events.Controllers
{
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

        // GET api/event
        [DataContextApiFilter]
        public PageableList<EventListItem> Get(DateTime? start, DateTime? end, int page, int pageSize)
        {
            return _ShowEventsService.ShowEvents(start, end, page, pageSize);
        }

        // GET api/event/5
        [DataContextApiFilter]
        public EventDetail Get(Guid eventCode)
        {
            return _ShowEventDetailService.ShowDetail(eventCode);
        }

        // POST api/event
        public void Post([FromBody]string name, [FromBody]string description, [FromBody]DateTime startDate, [FromBody]DateTime endDate)
        {
            var result = _CreateEventService.CreateNewEvent(name, description, startDate, endDate);
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
