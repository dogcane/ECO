using System;
using System.Web;

using ECO.Data;

namespace ECO.Integrations.Web
{
    public class DataContextModule : ContextLifeCicleModule, IHttpModule
    {
        #region Private_Fields

        private DataContext _Context;

        #endregion

        #region IHttpModule Members

        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.EndRequest += new EventHandler(context_EndRequest);
            context.PreRequestHandlerExecute += new EventHandler(context_PreRequestHandlerExecute);
            context.PostRequestHandlerExecute += new EventHandler(context_PostRequestHandlerExecute);
        }        

        #endregion

        #region Event_Handlers

        private void context_BeginRequest(object sender, EventArgs e)
        {
            OnContextPreInit(EventArgs.Empty);
            _Context = new DataContext();
            OnContextPostInit(EventArgs.Empty);        
        }

        private void context_EndRequest(object sender, EventArgs e)
        {            
            OnContextPreClose(EventArgs.Empty);
            _Context.Close();
            OnContextPostClose(EventArgs.Empty);
            _Context = null;        
        }

        private void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            OnContextPreExecute(EventArgs.Empty);
        }

        private void context_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            OnContextPostExecute(EventArgs.Empty);
        }        

        #endregion
    }
}
