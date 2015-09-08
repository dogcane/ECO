using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO.Data;
using Microsoft.Owin;

namespace ECO.Integrations.Owin
{
    public class ECOMiddleware : OwinMiddleware
    {
        public ECOMiddleware(OwinMiddleware next)
            : base(next)
        {

        }

        public override async Task Invoke(IOwinContext context)
        {
            using (DataContext ctx = new DataContext())
            {
                await Next.Invoke(context);
            }            
        }
    }    
}
