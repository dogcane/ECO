using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

using nh = NHibernate;
using nhcfg = NHibernate.Cfg;

using ECO;
using ECO.Context;
using ECO.Data;
using System.Reflection;
using NHibernate.Mapping.ByCode;

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

        private IList<Type> _Mappers = new List<Type>();

        #endregion

        #region ~Ctor

        public NHPersistenceUnit()
            : base()
        {

        }

        #endregion

        #region Protected_Methods

        protected virtual void TryAddClassMapping(nhcfg.Configuration cfg)
        {
            if (_Mappers.Count > 0)
            {
                ModelMapper mapper = new ModelMapper();
                mapper.AddMappings(_Mappers);
                cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
            }
        }

        protected virtual void BuildSessionFactory()
        {
            if (_SessionFactory == null)
            {
                nhcfg.Configuration cfg = new nhcfg.Configuration();
                cfg.Configure(_ConfigPath);
                TryAddClassMapping(cfg);
                _SessionFactory = cfg.BuildSessionFactory();
            }
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
        }

        protected override IPersistenceContext CreateContext()
        {
            BuildSessionFactory();
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

        public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>()
        {
            return new NHReadOnlyRepository<T, K>();
        }

        public override IRepository<T, K> BuildRepository<T, K>()
        {
            return new NHRepository<T, K>();
        }

        #endregion

        #region Public_Methods

        public NHPersistenceUnit AddMapping<T, K>()
            where T : IClassMapper<K>
            where K : class
        {
            _Mappers.Add(typeof(T));
            return this;
        }

        public NHPersistenceUnit RemoveMapping<T, K>()
            where T : IClassMapper<K>
            where K : class
        {
            _Mappers.Remove(typeof(T));
            return this;
        }

        public NHPersistenceUnit AddClassWithMapping<T, K, J>()
            where T : AggregateRoot<K>
            where J : IClassMapper<T>            
        {
            AddClass<T, K>();
            AddMapping<J, T>();
            return this;
        }

        public NHPersistenceUnit RemoveClassWithMapping<T, K, J>()
            where T : AggregateRoot<K>
            where J : IClassMapper<T>     
        {
            RemoveMapping<J, T>();
            RemoveClass<T, K>();
            return this;
        }

        #endregion
    }
}
