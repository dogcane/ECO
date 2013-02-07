using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Resources;

namespace ECO.Data
{
    public class PersistentClassNotRegistered : ApplicationException
    {
        #region Private_Fields

        private Type _PersistentClassType;

        #endregion

        #region Public_Properties

        public Type PersistenceClassType
        {
            get
            {
                return _PersistentClassType;
            }
        }

        #endregion

        #region Ctor

        public PersistentClassNotRegistered(Type persistenClassType)
            : base(string.Format(Errors.PERSISTENT_CLASS_NOT_FOUND, persistenClassType.Name))
        {
            _PersistentClassType = persistenClassType;
        }

        #endregion
    }
}
