using AutoMapper;
using ECO.Sample.Application.Speakers;
using ECO.Sample.Application.Speakers.Commands;
using ECO.Sample.Application.Speakers.Queries;
using ECO.Sample.Presentation.Areas.Speakers.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECO.Sample.Presentation.Areas.Speakers.Controllers
{
    [Area("speakers")]
    [Route("speakers")]
    public class SpeakerController : Controller
    {
        private IMediator _Mediator;

        private IMapper _Mapper;

        public SpeakerController(IMediator mediator, IMapper mapper)
        {
            _Mediator = mediator;
            _Mapper = mapper;
        }

        // GET: SpeakerController
        [HttpGet]
        [Route("")]
        [Route("index")]
        public async Task<ActionResult> Index(string nameOrSurname)
        {
            var result = await _Mediator.Send(new SearchSpeakers.Query(nameOrSurname));
            var model = new SpeakerListViewModel();
            if (result.Success)
            {
                model.Items = _Mapper.Map<IEnumerable<SpeakerItemViewModel>>(result.Value);
            }
            return View(model);
        }

        // GET: SpeakerController
        [HttpGet]
        [Route("json")]
        public async Task<ActionResult> JsonSearch(string nameOrSurname)
        {
            var result = await _Mediator.Send(new SearchSpeakers.Query(nameOrSurname));
            var model = new SpeakerListViewModel();
            if (result.Success)
            {
                model.Items = _Mapper.Map<IEnumerable<SpeakerItemViewModel>>(result.Value);
            }
            return Json(model);
        }

        // GET: SpeakerController/Create
        [HttpGet]
        [Route("create")]
        public ActionResult Create()
        {
            var model = SpeakerViewModel.Empty;
            return View(model);
        }

        // POST: SpeakerController/Create
        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SpeakerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _Mediator.Send(new CreateSpeaker.Command(model.Name, model.Surname, model.Description, model.Age));
            if (!result.Success)
            {
                result.Errors.ToList().ForEach(error => ModelState.AddModelError(error.Context, error.Description));
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: SpeakerController/Edit/5
        [HttpGet]
        [Route("edit/{id}")]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var result = await _Mediator.Send(new GetSpeaker.Query(id.Value));
            if (!result.Success)
                return NotFound();

            var model = _Mapper.Map<SpeakerViewModel>(result.Value);

            return View(model);
        }

        // POST: NewSpeakerController/Edit/5
        [HttpPost]
        [Route("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid? id, SpeakerViewModel model)
        {
            if (id == null)
                return NotFound();

            var result = await _Mediator.Send(new ChangeSpeaker.Command(model.SpeakerCode, model.Name, model.Surname, model.Description, model.Age));
            if (!result.Success)
            {
                result.Errors.ToList().ForEach(error => ModelState.AddModelError(error.Context, error.Description));
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: SpeakerController/Delete/5
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var result = await _Mediator.Send(new DeleteSpeaker.Command(id.Value));
            if (!result.Success)
                return RedirectToAction(nameof(Index), new { deleteError = true });

            return RedirectToAction(nameof(Index));
        }
    }
}
