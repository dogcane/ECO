using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Data;

namespace ECO.Providers.InMemory
{
    public class InMemoryPersistenceUnit : PersistenceUnitBase
    {
        #region Methods

        protected override IPersistenceContext CreateContext()
        {
            return new InMemoryPersistenceContext();
        }

        #endregion
    }
}
