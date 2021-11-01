using ECO.Resources;
using System;

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
