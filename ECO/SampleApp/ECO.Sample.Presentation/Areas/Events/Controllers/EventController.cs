using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ECO.Sample.Application.Events;
using ECO.Sample.Application.Events.DTO;
using ECO.Bender;
using ECO.Bender.Web;
using ECO.Bender.Web.MVC;
using ECO.Data;
using ECO.Web.MVC;
using System.Text;

namespace ECO.Sample.Presentation.Areas.Events.Controllers
{
    public class EventController : Controller
    {
        private IShowEventsService _ShowEventsService;

        private ICreateEventService _CreateEventService;

        private IShowEventDetailService _ShowEventDetailService;

        private IChangeEventService _ChangeEventService;

        private IDeleteEventService _DeleteEventService;

        private IAddSessionToEventService _AddSessionToEventService;

        private IRemoveSessionFromEventService _RemoveSessionFromEventService;

        public EventController(IShowEventsService showEventService, ICreateEventService createEventService,
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
       
        public ActionResult Index()
        {
            return View();
        }

        [DataContextFilter]
        public ActionResult Details(Guid id)
        {
            var model = _ShowEventDetailService.ShowDetail(id);
            return View(model);
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [DataContextFilter]
        public ActionResult Create(EventDetail model)
        {
            OperationResult result = _CreateEventService.CreateNewEvent(model.Name, model.Description, model.StartDate, model.EndDate);
            if (result.Success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState
                    .GetAdapter(result)                    
                    .AddResourceBinding("Event_Messages")
                    .Convert();
                return View();
            }
        }

        [DataContextFilter]
        public ActionResult Detail(Guid id)
        {
            var model = _ShowEventDetailService.ShowDetail(id);
            return View(model);
        }

        [HttpPost]
        [DataContextFilter]
        public ActionResult ChangeDates(Guid id, DateTime startDate, DateTime endDate)
        {
            OperationResult result = _ChangeEventService.ChangeDates(id, startDate, endDate);
            if (result.Success)
            {
                return RedirectToAction("Detail", new { id = id });
            }
            else
            {
                ModelState
                    .GetAdapter(result)                    
                    .AddResourceBinding("Event_Messages")
                    .Convert();
                var model = _ShowEventDetailService.ShowDetail(id);
                return View("Detail", model);
            }
        }

        [HttpPost]
        [DataContextFilter]
        public ActionResult ChangeInformations(Guid id, string name, string description)
        {
            OperationResult result = _ChangeEventService.ChangeInformation(id, name, description);
            if (result.Success)
            {
                return RedirectToAction("Detail", new { id = id });
            }
            else
            {
                ModelState
                    .GetAdapter(result)
                    .AddResourceBinding("Event_Messages")
                    .Convert();
                var model = _ShowEventDetailService.ShowDetail(id);
                return View("Detail", model);
            }
        }

        [DataContextFilter]
        public ActionResult Delete(Guid id)
        {
            OperationResult result = _DeleteEventService.DeleteEvent(id);
            if (!result.Success)
            {
                StringBuilder errors = new StringBuilder();
                result.Errors.ToList().ForEach(error => errors.AppendLine(error.Description));
                TempData.Add("ERROR", errors.ToString());
            }
            return RedirectToAction("Index");
        }
    }
}
