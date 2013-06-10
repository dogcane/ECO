using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Text;

using nh = NHibernate;
using nhcfg = NHibernate.Cfg;

using ECO;
using ECO.Context;
using ECO.Data;
using System.Reflection;

namespace ECO.Providers.NHibernate
{
    public class NHPersistenceUnit : PersistenceUnitBase
    {
        #region Consts

        private static readonly string CONFIGFILE_ATTRIBUTE = "configFile";

        private static readonly string INTERCEPTOR_ATTRIBUTE = "interceptor";

        #endregion

        #region Private_Fields

        private string _ConfigPath;

        private string _InterceptorFullName;

        private nh.ISessionFactory _SessionFactory;

        #endregion

        #region ~Ctor

        public NHPersistenceUnit()
            : base()
        {

        }

        #endregion

        #region Protected_Methods

        protected virtual void OnSetup(nh.ISessionFactory sessionFactory)
        {

        }

        protected override void OnInitialize(IDictionary<string, string> extendedAttributes)
        {
            base.OnInitialize(extendedAttributes);
            if (extendedAttributes.ContainsKey(CONFIGFILE_ATTRIBUTE))
            {
                _ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, extendedAttributes[CONFIGFILE_ATTRIBUTE]);
            }
            else
            {
                throw new ApplicationException(string.Format("The attribute '{0}' was not found in the persistent unit configuration", CONFIGFILE_ATTRIBUTE));
            }
            if (extendedAttributes.ContainsKey(INTERCEPTOR_ATTRIBUTE))
            {
                _InterceptorFullName = extendedAttributes[INTERCEPTOR_ATTRIBUTE];
            }
            _SessionFactory = new nhcfg.Configuration()
                .Configure(_ConfigPath)
                .BuildSessionFactory();
        }

        protected override IPersistenceContext CreateContext()
        {
            nh.ISession session = null;
            if (!string.IsNullOrEmpty(_InterceptorFullName))
            {
                Type interceptorType = Type.GetType(_InterceptorFullName);
                nh.IInterceptor interceptor = (nh.IInterceptor)Activator.CreateInstance(interceptorType);
                session = _SessionFactory.OpenSession(interceptor);
            }
            else
            {
                session = _SessionFactory.OpenSession();
            }
            return new NHPersistenceContext(session);
        }

        #endregion
    }
}
