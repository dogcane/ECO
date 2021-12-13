using AutoMapper;
using ECO.Sample.Application.Events.Commands;
using ECO.Sample.Application.Events.Queries;
using ECO.Sample.Presentation.Areas.Events.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Resulz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECO.Sample.Presentation.Areas.Events.Controllers
{
    [Area("events")]
    [Route("event")]
    public class EventController : Controller
    {
        private IMediator _Mediator;

        private IMapper _Mapper;

        public EventController(IMediator mediator, IMapper mapper)
        {
            _Mediator = mediator;
            _Mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult> Index(DateTime? start, DateTime? end, string eventName, bool error = false)
        {
            if (error) ViewBag.IsInError = true;

            var result = await _Mediator.Send(new SearchEvents.Query(start, end, eventName));
            var model = new EventListViewModel();
            if (result.Success)
            {
                model.Items = _Mapper.Map<IEnumerable<EventItemViewModel>>(result.Value);
            }
            return View(model);
        }

        [HttpGet("create")]
        public ActionResult Create()
        {
            var model = EventViewModel.Empty;
            return View(model);
        }

        [HttpPost("create")]
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

        [HttpGet("{id}/edit")]
        public async Task<ActionResult> Edit(Guid? id, bool error = false)
        {
            if (error) ViewBag.IsInError = true;

            if (id == null)
                return NotFound();

            var result = await _Mediator.Send(new GetEvent.Query(id.Value));
            if (!result.Success)
                return NotFound();

            var model = _Mapper.Map<EventViewModel>(result.Value);

            return View(model);
        }

        [HttpPost("{id}/edit")]
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

        [HttpGet]
        [Route("{id}/delete")]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var result = await _Mediator.Send(new DeleteEvent.Command(id.Value));
            if (!result.Success)
                return RedirectToAction(nameof(Index), new { error = true });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("{id}/addsession")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSession(Guid? id, EventViewModel model)
        {
            if (id == null)
                return NotFound();

            var result = await _Mediator.Send(new AddSessionToEvent.Command(model.EventCode, model.NewSession.Title, model.NewSession.Description, model.NewSession.Level, model.NewSession.SpeakerCode));
            if (!result.Success)
            {
                return RedirectToAction(nameof(Edit), new { id, error = true });
            }

            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpGet("{id}/removesession/{sessionid}")]
        public async Task<ActionResult> RemoveSession(Guid? id, Guid? sessionid)
        {
            if (id == null || sessionid == null)
                return NotFound();

            var result = await _Mediator.Send(new RemoveSessionFromEvent.Command(id.Value, sessionid.Value));
            if (!result.Success)
            {
                if (!result.Success)
                    return RedirectToAction(nameof(Edit), new { id, deleteError = true });
            }
            return RedirectToAction(nameof(Edit), new { id });
        }
    }
}
