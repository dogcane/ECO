using System.Web.Mvc;
using ECO.Data;

namespace ECO.Integrations.Web.MVC
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
