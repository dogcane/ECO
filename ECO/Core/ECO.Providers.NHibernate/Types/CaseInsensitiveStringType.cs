using NHibernate;
using NHibernate.Engine;
using System;

namespace ECO.Providers.NHibernate.Types
{
    public class CaseInsensitiveStringType : global::NHibernate.Type.AbstractStringType
    {
        #region Fields

        private StringComparer _Comparer = StringComparer.CurrentCultureIgnoreCase;

        #endregion

        public CaseInsensitiveStringType(global::NHibernate.SqlTypes.SqlType sqlType)
            : base(sqlType)
        {

        }

        public override string Name
        {
            get
            {
                return "String";
            }
        }

        public override bool IsEqual(object x, object y)
        {
            return _Comparer.Equals(x, y);
        }

        //public override bool IsEqual(object x, object y, EntityMode entityMode)
        //{
        //    return IsEqual(x, y);
        //}

        //public override bool IsEqual(object x, object y, EntityMode entityMode, ISessionFactoryImplementor factory)
        //{
        //    return IsEqual(x, y);
        //}

        //public override int GetHashCode(object x, EntityMode entityMode)
        //{
        //    return _Comparer.GetHashCode(x);
        //}

        //public override int GetHashCode(object x, EntityMode entityMode, ISessionFactoryImplementor factory)
        //{
        //    return GetHashCode(x, entityMode);
        //}
    }
}
