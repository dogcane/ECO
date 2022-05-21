using ECO.Data;
using System.Web.Http.Filters;

namespace ECO.Integrations.Web.MVC
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
