using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Resources;

namespace ECO.Data
{
    public class PersistenceContextNotFoundException : ApplicationException
    {
        #region Public_Properties

        public string PersistenceContextName { get; protected set; }

        #endregion

        #region Ctor

        public PersistenceContextNotFoundException(string persistenceContextName)
            : base(string.Format(Errors.PERSISTENCE_CONTEXT_NOT_FOUND, persistenceContextName))
        {
            PersistenceContextName = persistenceContextName;
        }

        #endregion
    }
}
