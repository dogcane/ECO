using ECO.Sample.Application.Events;
using ECO.Sample.Application.Events.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECO.Sample.Presentation.Areas.Events.Controllers
{
    [Area("events")]
    [Route("events/event")]
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

        // GET: EventController
        [HttpGet]
        [Route("")]
        [Route("index")]
        public ActionResult Index(DateTime? start, DateTime? end, string eventName)
        {
            var model = _ShowEventsService.ShowEvents(start, end, eventName);
            return View(model);
        }

        // GET: EventController/Create
        [HttpGet]
        [Route("create")]
        public ActionResult Create()
        {
            var model = new EventDetail();
            return View(model);
        }

        // POST: EventController/Create
        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventDetail model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = _CreateEventService.CreateNewEvent(model);
            if (!result.Success)
            {
                result.Errors.ToList().ForEach(error => ModelState.AddModelError(error.Context, error.Description));
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: EventController/Edit/5
        [HttpGet]
        [Route("edit/{id}")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var result = _ShowEventDetailService.GetEvent(id.Value);
            if (!result.Success)
                return NotFound();

            return View(result.Value);
        }

        // POST: NewEventController/Edit/5
        [HttpPost]
        [Route("edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid? id, EventDetail model)
        {
            if (id == null)
                return NotFound();

            var search = _ShowEventDetailService.GetEvent(id.Value);
            if (!search.Success)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var edit = _ChangeEventService.ChangeInformation(model);
            if (!edit.Success)
            {
                edit.Errors.ToList().ForEach(error => ModelState.AddModelError(error.Context, error.Description));
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: EventController/Delete/5
        [HttpGet]
        [Route("delete/{id}")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var search = _ShowEventDetailService.GetEvent(id.Value);
            if (!search.Success)
                return NotFound();

            var delete = _DeleteEventService.DeleteEvent(id.Value);
            if (!delete.Success)
                return RedirectToAction(nameof(Index), new { deleteError = true });

            return RedirectToAction(nameof(Index));
        }
    }
}
