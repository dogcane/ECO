using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO.Data;

namespace ECO.Integrations.CastleWindsor.Interception
{
    public class DataContextInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            DataContext ctx = null;
            TransactionContext tx = null;
            try
            {
                ctx = new DataContext();
                tx = ctx.BeginTransaction();
                invocation.Proceed();
                tx.Commit();
            }
            catch
            {
                if (tx != null && tx.Status == TransactionStatus.Alive)
                {
                    tx.Rollback();
                }
                throw;
            }
            finally
            {
                if (tx != null)
                {
                    tx.Dispose();
                }
                if (ctx != null)
                {
                    ctx.Close();
                }
            }
        }
    }
}
