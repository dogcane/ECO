using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECO;
using ECO.Resources;

namespace ECO.Data
{
    public class PersistenceUnitNotFoundException : ApplicationException
    {
        #region Public_Properties

        public string PersistenceUnitName { get; protected set; }

        #endregion

        #region Ctor

        public PersistenceUnitNotFoundException(string persistenceUnitName)
            : base(string.Format(Errors.PERSISTENCE_UNIT_NOT_FOUND, persistenceUnitName))
        {
            PersistenceUnitName = persistenceUnitName;
        }

        #endregion
    }
}
