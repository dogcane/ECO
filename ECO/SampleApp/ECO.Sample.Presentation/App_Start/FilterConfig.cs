using System.Web;
using System.Web.Mvc;

using ECO.Web.MVC;

namespace ECO.Sample.Presentation
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new DataContextFilter()); You can also abilitate global DataContextFilter
        }
    }
}