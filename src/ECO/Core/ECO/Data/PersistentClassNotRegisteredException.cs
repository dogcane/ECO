using System;

namespace ECO.Data
{
    public class PersistentClassNotRegisteredException : ApplicationException
    {
        #region Public_Properties

        public Type PersistentClassType { get; protected set; }

        #endregion

        #region Ctor

        public PersistentClassNotRegisteredException(Type persistenClassType)
            : base($"Persistent class '{persistenClassType.Name}' not registered") => PersistentClassType = persistenClassType;

        #endregion
    }
}
