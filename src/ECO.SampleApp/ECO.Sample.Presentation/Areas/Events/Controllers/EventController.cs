using ECO.Sample.Application.Events;
using ECO.Sample.Application.Events.Commands;
using ECO.Sample.Application.Events.Queries;
using ECO.Sample.Presentation.Areas.Events.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECO.Sample.Presentation.Areas.Events.Controllers
{
    [Area("events")]
    [Route("events")]
    public class EventController : Controller
    {
        private IMediator _Mediator;

        public EventController(IMediator mediator)
        {
            _Mediator = mediator;
        }

        // GET: EventController
        [HttpGet]
        [Route("")]
        [Route("index")]
        public async Task<ActionResult> Index(DateTime? start, DateTime? end, string eventName)
        {
            var result = await _Mediator.Send(new SearchEvents.Query(start, end, eventName));
            var model = new EventListViewModel();
            if (result.Success) {
                model.Items = result.Value.Select(item => new EventItemViewModel
                {
                    EventCode = item.EventCode,
                    Name = item.Name,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    NumberOfSessions = item.NumberOfSessions
                });                
            }
            return View(model);
        }

        // GET: EventController/Create
        [HttpGet]
        [Route("create")]
        public ActionResult Create()
        {
            var model = EventViewModel.Empty;
            return View(model);
        }

        // POST: EventController/Create
        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EventViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _Mediator.Send(new CreateEvent.Command(model.Name, model.Description, model.StartDate, model.EndDate));
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
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var result = await _Mediator.Send(new GetEvent.Query(id.Value));
            if (!result.Success)
                return NotFound();

            var model = new EventViewModel
            {
                EventCode = result.Value.EventCode,
                Description = result.Value.Description,
                StartDate = result.Value.StartDate,
                EndDate = result.Value.EndDate,
                Name = result.Value.Name,
                Sessions = result.Value.SessionItems.Select(item => new SessionItemViewModel { 
                    Level = item.Level,
                    Title = item.Title,
                    Speaker = item.Speaker,
                    SessionCode = item.SessionCode
                })
            };

            return View(model);
        }

        // POST: NewEventController/Edit/5
        [HttpPost]
        [Route("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid? id, EventViewModel model)
        {
            if (id == null)
                return NotFound();

            var result = await _Mediator.Send(new ChangeEvent.Command(model.EventCode, model.Name, model.Description, model.StartDate, model.EndDate));
            if (!result.Success)
            {
                result.Errors.ToList().ForEach(error => ModelState.AddModelError(error.Context, error.Description));
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: EventController/Delete/5
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var result = await _Mediator.Send(new DeleteEvent.Command(id.Value));
            if (!result.Success)
                return RedirectToAction(nameof(Index), new { deleteError = true });

            return RedirectToAction(nameof(Index));
        }
    }
}
