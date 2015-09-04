using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO;
using ECO.Data;

namespace ECO.Providers.EntityFramework
{
    public class EntityFrameworkPersistenceUnit : PersistenceUnitBase
    {
        protected override IPersistenceContext CreateContext()
        {
            throw new NotImplementedException();
        }

        public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>()
        {
            throw new NotImplementedException();
        }

        public override IRepository<T, K> BuildRepository<T, K>()
        {
            throw new NotImplementedException();
        }
    }
}
