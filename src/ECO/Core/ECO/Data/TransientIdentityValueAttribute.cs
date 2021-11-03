using System;

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

        public Type IdentityType => _IdentityType;

        public object DefaultValue => _DefaultValue;

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
