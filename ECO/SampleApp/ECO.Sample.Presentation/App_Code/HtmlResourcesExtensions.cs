using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECO.Sample.Presentation.App_Code
{
    public static class HtmlResourcesExtensions
    {
        public static HtmlString Resx(this RazorView view, string resourceName)
        {
            return new HtmlString(HttpContext.GetLocalResourceObject(view.ViewPath, "Name").ToString());
        }

        public static HtmlString Resx(this RazorView html, string globalResourceFile, string resourceName)
        {
            return new HtmlString(HttpContext.GetGlobalResourceObject(globalResourceFile, resourceName).ToString());
        }
    }
}