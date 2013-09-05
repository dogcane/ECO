using System.Web.Mvc;
using System.Web.Http;

using AttributeRouting;
using AttributeRouting.Web.Http;
using AttributeRouting.Helpers;

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
            //Default Speakers Controllers
            context.MapRoute(
                "Speakers_default",
                "speakers",
                new { controller = "Speaker", action = "Index", id = UrlParameter.Optional, area = "Speakers" }
            );
        }
    }
}
