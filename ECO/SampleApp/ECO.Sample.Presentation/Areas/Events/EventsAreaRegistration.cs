using System.Web.Mvc;

namespace ECO.Sample.Presentation.Areas.Events
{
    public class EventsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Events";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //List of events
            context.MapRoute(
                "Events",
                "events", new { controller = "Event", action = "Index", area = "Events" });

            //Create new event
            context.MapRoute(
                "AddEvent",
                "events/new", new { controller = "Event", action = "Create", area = "Events" });

            //View event
            context.MapRoute(
                "ViewEvent",
                "events/{id}/view", new { controller = "Event", action = "Detail", area = "Events" });

            //Delete event
            context.MapRoute(
                "DeleteEvent",
                "events/{id}/remove", new { controller = "Event", action = "Delete", area = "Events" });

            //Update event informations
            context.MapRoute(
                "UpdateEventInformations",
                "events/{id}/updateinfo", new { controller = "Event", action = "ChangeInformations", area = "Events" });

            //Update event dates
            context.MapRoute(
                "UpdateEventDates",
                "events/{id}/updatedates", new { controller = "Event", action = "ChangeDates", area = "Events" });
        }
    }
}
