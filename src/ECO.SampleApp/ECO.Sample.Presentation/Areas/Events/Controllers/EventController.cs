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
using ECO.Integrations.Web.MVC;
using System.Text;

namespace ECO.Sample.Presentation.Areas.Events.Controllers
{
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
       
        public ActionResult Index()
        {
            return View();
        }
    }
}
