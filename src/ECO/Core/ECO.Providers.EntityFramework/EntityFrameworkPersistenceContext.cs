using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO;
using ECO.Data;

namespace ECO.Providers.EntityFramework
{
    public class EntityFrameworkPersistenceContext : PersistentContextBase
    {
        #region Public_Properties

        public DbContext Context { get; protected set; }

        #endregion

        #region ~Ctor

        public EntityFrameworkPersistenceContext(DbContext context)
        {
            Context = context;
        }

        ~EntityFrameworkPersistenceContext()
        {
            Dispose(false);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            if (Transaction != null)
            {
                Transaction.Dispose();
            }
            else
            {
                Context.SaveChanges();
            }
        }

        #endregion

        #region Public_Methods

        protected override IDataTransaction OnBeginTransaction()
        {
            Transaction = new EntityFrameworkDataTransaction(this);
            return Transaction;
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            Context.SaveChanges();
        }

        #endregion
    }
}
