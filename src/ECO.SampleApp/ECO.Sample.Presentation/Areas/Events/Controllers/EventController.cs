using ECO.Sample.Application.Events;
using ECO.Sample.Application.Events.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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

        // GET: NewEventController
        public ActionResult Index(DateTime? start, DateTime? end, string eventName)
        {
            var model = _ShowEventsService.ShowEvents(start, end, eventName);
            return View(model);
        }

        // GET: NewEventController/Create
        public ActionResult Create()
        {
            var model = new EventDetail();
            return View(model);
        }

        // POST: NewEventController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventDetail model)
        {
            try
            {
                var result = _CreateEventService.CreateNewEvent(model);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }
    }
}
