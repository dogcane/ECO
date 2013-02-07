using System;
using System.Collections.Generic;
using System.Web;
using System.Text;

using ECO;
using ECO.Data;

namespace ECO.Web
{
    public class UnitOfWorkModule : ContextLifeCicleModule, IHttpModule
    {
        #region Private_Fields

        private UnitOfWork _UnitOfWork;

        #endregion

        #region IHttpModule Members

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.EndRequest += new EventHandler(context_EndRequest);
            context.Error += new EventHandler(context_Error);
        }        

        #endregion        

        #region Event_Handlers

        private void context_BeginRequest(object sender, EventArgs e)
        {
            OnContextPreInit(EventArgs.Empty);
            _UnitOfWork = new UnitOfWork();
            OnContextPostInit(EventArgs.Empty);
        }

        private void context_EndRequest(object sender, EventArgs e)
        {
            if (_UnitOfWork != null)
            {
                OnContextPreClose(EventArgs.Empty);
                _UnitOfWork.Commit();
                _UnitOfWork.Close();                
                OnContextPostClose(EventArgs.Empty);
                _UnitOfWork = null;
            }
        }

        private void context_Error(object sender, EventArgs e)
        {
            if (_UnitOfWork != null)
            {
                OnContextPreClose(EventArgs.Empty);
                _UnitOfWork.Rollback();
                _UnitOfWork.Close();
                OnContextPostClose(EventArgs.Empty);
                _UnitOfWork = null;
            }
        }

        #endregion
    }
}
