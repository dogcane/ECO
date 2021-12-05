using ECO.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using nh = NHibernate;
using nhcfg = NHibernate.Cfg;

namespace ECO.Providers.NHibernate
{
    public sealed class NHPersistenceUnit : PersistenceUnitBase<NHPersistenceUnit>
    {
        #region Consts
        
        private static readonly string SESSIONINTERCEPTOR_ATTRIBUTE = "sessionInterceptor";

        private static readonly string MAPPINGASSEMBLIES_ATTRIBUTE = "mappingAssemblies";

        #endregion

        #region Private_Fields

        private IDictionary<string, string> _ConfigurationProperties = new Dictionary<string, string>();

        private nh.ISessionFactory _SessionFactory;

        #endregion

        #region Ctor

        public NHPersistenceUnit(string name, ILoggerFactory loggerFactory) : base(name, loggerFactory)
        {

        }

        #endregion

        #region Private_Methods
        private void BuildSessionFactory()
        {
            if (_SessionFactory == null)
            {
                var configuration = new nhcfg.Configuration();
                configuration.AddProperties(_ConfigurationProperties);
                if (_ConfigurationProperties.ContainsKey(SESSIONINTERCEPTOR_ATTRIBUTE))
                {
                    Type interceptorType = Type.GetType(_ConfigurationProperties[SESSIONINTERCEPTOR_ATTRIBUTE]);
                    nh.IInterceptor interceptor = Activator.CreateInstance(interceptorType) as nh.IInterceptor;
                    configuration.SetInterceptor(interceptor);
                }
                if (_ConfigurationProperties.ContainsKey(MAPPINGASSEMBLIES_ATTRIBUTE))
                {
                    var mappingAssemblies = _ConfigurationProperties[MAPPINGASSEMBLIES_ATTRIBUTE].Split(";", StringSplitOptions.RemoveEmptyEntries);
                    //Hbm.xml
                    foreach (var mappingAssembly in mappingAssemblies)
                    {
                        configuration.AddAssembly(mappingAssembly);
                    }
                    //ClassMapping
                    var mapper = new nh.Mapping.ByCode.ModelMapper();
                    foreach (var mappingAssembly in mappingAssemblies)
                    {
                        mapper.AddMappings(Assembly.Load(mappingAssembly).ExportedTypes);
                    }
                    nhcfg.MappingSchema.HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
                    configuration.AddMapping(domainMapping);
                }
                _SessionFactory = configuration.BuildSessionFactory();
                //Register class types
                foreach (var classMetadata in _SessionFactory.GetAllClassMetadata())
                {
                    var mappedClass = classMetadata.Value.MappedClass;
                    if (mappedClass.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAggregateRoot<>)))
                        _Classes.Add(classMetadata.Value.MappedClass);
                }
            }
        }

        #endregion

        #region Protected_Methods

        protected override void OnInitialize(IDictionary<string, string> extendedAttributes)
        {
            base.OnInitialize(extendedAttributes);
            _ConfigurationProperties = extendedAttributes;
            BuildSessionFactory();
        }

        protected override IPersistenceContext OnCreateContext()
        {            
            nh.ISession session = _SessionFactory.OpenSession();
            return new NHPersistenceContext(session, this, _LoggerFactory);
        }

        #endregion

        #region Public_Methods

        public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext)
        {
            return new NHReadOnlyRepository<T, K>(dataContext);
        }

        public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext)
        {
            return new NHRepository<T, K>(dataContext);
        }

        #endregion
    }
}
