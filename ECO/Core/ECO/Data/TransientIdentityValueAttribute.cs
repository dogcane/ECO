using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECO.Data
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class TransientIdentityValueAttribute : Attribute
    {
        #region Private_Fields

        private Type _IdentityType;

        private object _DefaultValue;

        #endregion

        #region Public_Properties

        public Type IdentityType
        {
            get
            {
                return _IdentityType;
            }
        }

        public object DefaultValue
        {
            get
            {
                return _DefaultValue;
            }
        }

        #endregion

        #region Ctor

        public TransientIdentityValueAttribute(Type identityType, object defaultValue)
        {
            _IdentityType = identityType;
            _DefaultValue = defaultValue;
        }

        #endregion
    }
}
