using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Resources;

namespace ECO.Data
{
    public class PersistenceContextNotFound : ApplicationException
    {
        #region Private_Fields

        private string _PersistenceContextName;

        #endregion

        #region Public_Properties

        public string PersistenceContextName
        {
            get
            {
                return _PersistenceContextName;
            }
        }

        #endregion

        #region Ctor

        public PersistenceContextNotFound(string persistenceContextName)
            : base(string.Format(Errors.PERSISTENCE_CONTEXT_NOT_FOUND, persistenceContextName))
        {
            _PersistenceContextName = persistenceContextName;
        }

        #endregion
    }
}
