using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Http.Filters;
using ECO.Data;

namespace ECO.Web.MVC
{
    public class DataContextApiFilter : ActionFilterAttribute
    {
        public bool RequiredTransaction = false;

        public bool AutoCommitTransaction = true;

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            var dataContext = new DataContext();
            if (RequiredTransaction)
            {
                dataContext.BeginTransaction(AutoCommitTransaction);
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            DataContext.Current.Close();
        }        
    }
}
