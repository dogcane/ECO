using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Mvc;
using ECO.Data;

namespace ECO.Web.MVC
{
    public class DataContextFilter : ActionFilterAttribute
    {
        public bool RequiredTransaction = false;

        public bool AutoCommitTransaction = true;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var dataContext = new DataContext();
            if (RequiredTransaction)
            {
                dataContext.BeginTransaction(AutoCommitTransaction);
            }
        }        

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            DataContext.Current.Close();
        }
    }
}
