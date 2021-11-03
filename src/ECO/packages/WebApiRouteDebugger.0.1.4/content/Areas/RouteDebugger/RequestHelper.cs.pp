using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using $rootnamespace$.Areas.RouteDebugger.Components;

namespace $rootnamespace$.Areas.RouteDebugger
{
    public static class RequestHelper
    {
        public static readonly string InspectHeaderName = "RouteInspecting";
        public static readonly string RouteDataCache = "RD_ROUTEDATA";
        public static readonly string RoutesCache = "RD_ROUTES";
        public static readonly string ControllerCache = "RD_CONTROLLER";
        public static readonly string ActionCache = "RD_ACTION";
        public static readonly string SelectedController = "RD_SELECTED_CONTROLLER";

        /// <summary>
        /// Returns true if this request is a inspect request. 
        /// 
        /// For sake of security, route debugging will obey include error detail policy.
        /// </summary>
        public static bool IsInspectRequest(this HttpRequestMessage request)
        {
            IEnumerable<string> values;

            if (request.Headers.TryGetValues(InspectHeaderName, out values) && values.Contains("true"))
            {
                return request.ShouldIncludeErrorDetail();
            }

            return false;
        }
    }
}
