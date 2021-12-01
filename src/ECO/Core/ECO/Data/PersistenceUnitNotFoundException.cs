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
            : base($"Persistence Unit '{persistenceUnitName}' not found") => PersistenceUnitName = persistenceUnitName;

        #endregion
    }
}
