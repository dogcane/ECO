using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO;
using ECO.Data;

namespace ECO.Providers.EntityFramework
{
    public class EntityFrameworkPersistenceContext : PersistentContextBase
    {
        protected override IDataTransaction OnBeginTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
