using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace ECO.Sample.Presentation.Areas.RouteDebugger.Components
{
    public static class RequestExtension
    {
        public static bool ShouldIncludeErrorDetail(this HttpRequestMessage request)
        {
            switch (request.GetConfiguration().IncludeErrorDetailPolicy)
            {
                case IncludeErrorDetailPolicy.Default:
                    Lazy<bool> includeErrorDetail;
                    if (request.Properties.TryGetValue<Lazy<bool>>(HttpPropertyKeys.IncludeErrorDetailKey, out includeErrorDetail))
                    {
                        // If we are on webhost and the user hasn't changed the IncludeErrorDetailPolicy
                        // look up into the ASP.NET CustomErrors property else default to LocalOnly.
                        return includeErrorDetail.Value;
                    }

                    goto case IncludeErrorDetailPolicy.LocalOnly;

                case IncludeErrorDetailPolicy.LocalOnly:
                    if (request == null)
                    {
                        return false;
                    }

                    Lazy<bool> isLocal;
                    if (request.Properties.TryGetValue<Lazy<bool>>(HttpPropertyKeys.IsLocalKey, out isLocal))
                    {
                        return isLocal.Value;
                    }

                    return false;

                case IncludeErrorDetailPolicy.Always:
                    return true;

                case IncludeErrorDetailPolicy.Never:
                default:
                    return false;
            }
        }
    }
}
