using ECO.Data;
using Microsoft.Extensions.Configuration;
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

        private nh.ISessionFactory? _SessionFactory;

        #endregion

        #region Ctor

        public NHPersistenceUnit(string name, ILoggerFactory? loggerFactory = null) : base(name, loggerFactory)
        {

        }

        #endregion

        #region Private_Methods
        private void BuildSessionFactory(IConfiguration appConfiguration)
        {
            if (_SessionFactory == null)
            {
                var nhConfiguration = new nhcfg.Configuration();
                if (_ConfigurationProperties.TryGetValue(nhcfg.Environment.ConnectionStringName, out var connectionStringName))
                {
                    var connectionString = appConfiguration.GetConnectionString(connectionStringName);
                    if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException($"The connection string {connectionStringName} is not defined in the configuration file");
                    nhConfiguration.SetProperty(nhcfg.Environment.ConnectionString, connectionString);
                }
                nhConfiguration.AddProperties(_ConfigurationProperties);
                if (_ConfigurationProperties.ContainsKey(SESSIONINTERCEPTOR_ATTRIBUTE))
                {
                    Type interceptorType = Type.GetType(_ConfigurationProperties[SESSIONINTERCEPTOR_ATTRIBUTE]);
                    if (Activator.CreateInstance(interceptorType) is nh.IInterceptor interceptor)
                        nhConfiguration.SetInterceptor(interceptor);
                }
                if (_ConfigurationProperties.ContainsKey(MAPPINGASSEMBLIES_ATTRIBUTE))
                {
                    var mappingAssemblies = _ConfigurationProperties[MAPPINGASSEMBLIES_ATTRIBUTE].Split(";", StringSplitOptions.RemoveEmptyEntries);
                    //Hbm.xml
                    foreach (var mappingAssembly in mappingAssemblies)
                    {
                        nhConfiguration.AddAssembly(mappingAssembly);
                    }
                    //ClassMapping
                    var mapper = new nh.Mapping.ByCode.ModelMapper();
                    foreach (var mappingAssembly in mappingAssemblies)
                    {
                        mapper.AddMappings(Assembly.Load(mappingAssembly).ExportedTypes);
                    }
                    nhcfg.MappingSchema.HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
                    nhConfiguration.AddMapping(domainMapping);
                }
                _SessionFactory = nhConfiguration.BuildSessionFactory();
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

        protected override void OnInitialize(IDictionary<string, string> extendedAttributes, IConfiguration configuration)
        {
            base.OnInitialize(extendedAttributes, configuration);
            _ConfigurationProperties = extendedAttributes;
            BuildSessionFactory(configuration);
        }

        protected override IPersistenceContext OnCreateContext()
        {
            if (_SessionFactory == null) throw new ArgumentException();
            return new NHPersistenceContext(_SessionFactory.OpenSession(), this, _LoggerFactory?.CreateLogger<NHPersistenceContext>());
        }

        #endregion

        #region Public_Methods

        public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext) => new NHReadOnlyRepository<T, K>(dataContext);

        public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext) => new NHRepository<T, K>(dataContext);

        #endregion
    }
}
