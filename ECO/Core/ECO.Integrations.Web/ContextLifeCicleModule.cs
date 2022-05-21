using System;
using System.Collections.Generic;
using System.Web;
using System.Text;

using ECO;
using ECO.Data;

namespace ECO.Integrations.Web
{
    public abstract class ContextLifeCicleModule
    {
        #region Protected_Methods

        protected virtual void OnContextPreInit(EventArgs args)
        {

        }

        protected virtual void OnContextInit(EventArgs args)
        {

        }

        protected virtual void OnContextPostInit(EventArgs args)
        {

        }

        protected virtual void OnContextPreExecute(EventArgs args)
        {

        }

        protected virtual void OnContextPostExecute(EventArgs args)
        {

        }

        protected virtual void OnContextPreClose(EventArgs args)
        {

        }

        protected virtual void OnContextPostClose(EventArgs args)
        {

        }

        #endregion
    }
}
