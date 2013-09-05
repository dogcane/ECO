using System.Web.Http;
using AttributeRouting.Web.Http.WebHost;

[assembly: WebActivator.PreApplicationStartMethod(typeof(ECO.Sample.Presentation.AttributeRoutingHttpConfig), "Start")]

namespace ECO.Sample.Presentation 
{
    public static class AttributeRoutingHttpConfig
	{
		public static void RegisterRoutes(HttpRouteCollection routes) 
		{    
            routes.MapHttpAttributeRoutes();
		}

        public static void Start() 
		{
            RegisterRoutes(GlobalConfiguration.Configuration.Routes);
        }
    }
}
