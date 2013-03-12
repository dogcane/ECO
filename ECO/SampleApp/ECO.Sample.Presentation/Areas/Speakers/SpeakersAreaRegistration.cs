using System.Web.Mvc;
using System.Web.Http;

namespace ECO.Sample.Presentation.Areas.Speakers
{
    public class SpeakersAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Speakers";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //Speakers API
            context.Routes.MapHttpRoute(
                name: "Speakers Api",
                routeTemplate: "speakers/api/{id}",
                defaults: new { controller = "SpeakerApi", id = RouteParameter.Optional }
            );

            //Default Speakers Controllers
            context.MapRoute(
                "Speakers_default",
                "speakers",
                new { controller = "Speaker", action = "Index", id = UrlParameter.Optional, area = "Speakers" }
            );
        }
    }
}
