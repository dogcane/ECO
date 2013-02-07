using System.Web.Mvc;

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
            context.MapRoute(
                "Speakers_default",
                "Speakers/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
