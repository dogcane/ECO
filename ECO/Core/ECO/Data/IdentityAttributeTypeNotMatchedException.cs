using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Resources;

namespace ECO.Data
{
    public class IdentityAttributeTypeNotMatchedException : ApplicationException
    {
        #region Private_Fields

        private Type _EntityType;

        #endregion

        #region Public_Properties

        public Type EntityType
        {
            get
            {
                return _EntityType;
            }
        }

        #endregion

        #region Ctor

        public IdentityAttributeTypeNotMatchedException(Type entityType)
            : base(string.Format(Errors.IDENTITY_TYPE_NOT_MATCHED, entityType.Name))
        {
            _EntityType = entityType;
        }

        #endregion
    }
}
