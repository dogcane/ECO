using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECO.Configuration
{
    public class PersistenceContextAttribute : Attribute
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

        public PersistenceContextAttribute(string persistenceContextName)
        {
            _PersistenceContextName = persistenceContextName;
        }

        #endregion
    }
}
