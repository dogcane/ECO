using System;
using System.Collections.Generic;
using System.Web;
using System.Text;

using ECO;
using ECO.Data;

namespace ECO.Web
{
    public class TransactionalDataContextModule : ContextLifeCicleModule, IHttpModule
    {
        #region Private_Fields

        private DataContext _DataContext;

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
            _DataContext = new DataContext();
            _DataContext.BeginTransaction();
            OnContextPostInit(EventArgs.Empty);
        }

        private void context_EndRequest(object sender, EventArgs e)
        {
            if (_DataContext != null)
            {
                OnContextPreClose(EventArgs.Empty);
                if (_DataContext.Transaction != null)
                {
                    _DataContext.Transaction.Commit();
                }
                _DataContext.Close();
                OnContextPostClose(EventArgs.Empty);
            }
        }

        private void context_Error(object sender, EventArgs e)
        {
            if (_DataContext != null)
            {
                OnContextPreClose(EventArgs.Empty);
                if (_DataContext.Transaction != null)
                {
                    _DataContext.Transaction.Rollback();
                }
                _DataContext.Close();
                OnContextPostClose(EventArgs.Empty);                
            }
        }

        #endregion
    }
}
