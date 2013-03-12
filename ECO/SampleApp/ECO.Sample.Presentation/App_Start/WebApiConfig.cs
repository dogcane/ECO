using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ECO.Sample.Presentation
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Events API
            config.Routes.MapHttpRoute(
                name: "Events List Api",
                routeTemplate: "events/api/{id}",
                defaults: new { controller = "EventApi", id = RouteParameter.Optional, Area = "Events" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );            
        }
    }
}
