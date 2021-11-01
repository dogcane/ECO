using ECO.Resources;
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
            : base(string.Format(Errors.PERSISTENT_CLASS_NOT_FOUND, persistenClassType.Name))
        {
            PersistentClassType = persistenClassType;
        }

        #endregion
    }
}
