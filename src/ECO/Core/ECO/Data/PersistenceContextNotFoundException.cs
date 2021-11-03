using System;

namespace ECO.Data
{
    public class PersistenceContextNotFoundException : ApplicationException
    {
        #region Public_Properties

        public string PersistenceContextName { get; protected set; }

        #endregion

        #region Ctor

        public PersistenceContextNotFoundException(string persistenceContextName)
            : base($"The persistence context '{persistenceContextName}' was not found")
        {
            PersistenceContextName = persistenceContextName;
        }

        #endregion
    }
}
