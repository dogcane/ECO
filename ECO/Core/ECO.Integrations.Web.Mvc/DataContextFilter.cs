using System.Data;
using System.Web.Mvc;
using ECO.Data;

namespace ECO.Integrations.Web.MVC
{
    public class DataContextFilter : ActionFilterAttribute, IExceptionFilter
    {
        public bool RequiredTransaction = false;

        public bool AutoCommitTransaction = true;

        public IsolationLevel? TransactionLevel = null;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (!filterContext.IsChildAction)
            {
                var dataContext = new DataContext();
                if (RequiredTransaction)
                {
                    if (TransactionLevel.HasValue)
                    {
                        dataContext.BeginTransaction(AutoCommitTransaction, TransactionLevel.Value);
                    }
                    else
                    {
                        dataContext.BeginTransaction(AutoCommitTransaction);
                    }
                }
            }
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            if (!filterContext.IsChildAction)
            {
                if (DataContext.Current != null)
                {
                    DataContext.Current.Close();
                }
            }
        }

        #region IExceptionFilter Members

        public void OnException(ExceptionContext filterContext)
        {
            if (DataContext.Current != null)
            {
                DataContext.Current.Close();
            }
        }

        #endregion
    }
}
