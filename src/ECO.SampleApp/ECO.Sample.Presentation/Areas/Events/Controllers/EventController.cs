using ECO.Sample.Application.Events;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ECO.Sample.Presentation.Areas.Events.Controllers
{
    [Area("events")]
    public class EventController : Controller
    {
        private IShowEventsService _ShowEventsService;

        private ICreateEventService _CreateEventService;

        private IGetEventService _ShowEventDetailService;

        private IChangeEventService _ChangeEventService;

        private IDeleteEventService _DeleteEventService;

        private IAddSessionToEventService _AddSessionToEventService;

        private IRemoveSessionFromEventService _RemoveSessionFromEventService;

        public EventController(IShowEventsService showEventService, ICreateEventService createEventService,
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
       
        public ActionResult Index(DateTime? start, DateTime? end, string eventName)
        {
            var model = _ShowEventsService.ShowEvents(start, end, eventName);
            return View(model);
        }
    }
}
