using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;

namespace ECO.Integrations.Owin
{
    public static class ECOAppBuilderExtensions
    {
        public static void UseECO(this IAppBuilder app)
        {
            app.Use<ECOMiddleware>();
        }
    }
}
