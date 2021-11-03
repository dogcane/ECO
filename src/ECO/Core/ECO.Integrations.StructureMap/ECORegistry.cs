using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace ECO.Integrations.StructureMap
{
    public class ECORegistry : Registry
    {
        public ECORegistry()
        {
            For(typeof(IReadOnlyRepository<,>)).Add(x => BuildReadOnlyRepository(x));
            For(typeof(IRepository<,>)).Add(x => BuildRepository(x));
        }

        private object BuildReadOnlyRepository(IContext context)
        {
            throw new NotImplementedException();
        }

        private object BuildRepository(IContext context)
        {
            throw new NotImplementedException();
        }
    }
}
