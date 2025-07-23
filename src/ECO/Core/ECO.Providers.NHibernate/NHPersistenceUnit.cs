namespace ECO.Providers.NHibernate;

using ECO.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nh = global::NHibernate;
using NhCfg = global::NHibernate.Cfg;

public sealed class NHPersistenceUnit(string name, ILoggerFactory? loggerFactory = null) : PersistenceUnitBase<NHPersistenceUnit>(name, loggerFactory)
{
    #region Consts    
    private static readonly string SESSIONINTERCEPTOR_ATTRIBUTE = "sessionInterceptor";
    private static readonly string MAPPINGASSEMBLIES_ATTRIBUTE = "mappingAssemblies";
    #endregion

    #region Private_Fields
    private Dictionary<string, string> _ConfigurationProperties = [];
    private Nh.ISessionFactory? _SessionFactory;
    #endregion

    #region Private_Methods
    private void BuildSessionFactory(IConfiguration appConfiguration)
    {
        if (_SessionFactory == null)
        {
            var nhConfiguration = new NhCfg.Configuration();
            //Fix for ConnectionStringName
            if (_ConfigurationProperties.TryGetValue(NhCfg.Environment.ConnectionStringName, out var connectionStringName))
            {
                var connectionString = appConfiguration.GetConnectionString(connectionStringName);
                if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException($"The connection string {connectionStringName} is not defined in the configuration file");
                nhConfiguration.SetProperty(NhCfg.Environment.ConnectionString, connectionString);
                _ConfigurationProperties.Remove(NhCfg.Environment.ConnectionStringName);
            }
            //Fix for generic connection strings
            foreach (var (key,val) in _ConfigurationProperties)
            {
                if (val.StartsWith("connectionStringName.", StringComparison.OrdinalIgnoreCase))
                {
                    var connectionString = appConfiguration.GetConnectionString(val.Replace("connectionStringName.", ""));
                    if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException($"The connection string {val} is not defined in the configuration file");
                    _ConfigurationProperties[key] = connectionString;
                }
            }
            nhConfiguration.AddProperties(_ConfigurationProperties);
            if (_ConfigurationProperties.TryGetValue(SESSIONINTERCEPTOR_ATTRIBUTE, out var sessionInterceptor))
            {
                var interceptorType = Type.GetType(sessionInterceptor, false, true);
                if (interceptorType != null && Activator.CreateInstance(interceptorType) is Nh.IInterceptor interceptor)
                    nhConfiguration.SetInterceptor(interceptor);
            }
            if (_ConfigurationProperties.TryGetValue(MAPPINGASSEMBLIES_ATTRIBUTE, out string? value))
            {
                var mappingAssemblies = value.Split(";", StringSplitOptions.RemoveEmptyEntries);
                //Hbm.xml
                foreach (var mappingAssembly in mappingAssemblies)
                {
                    nhConfiguration.AddAssembly(mappingAssembly);
                }
                //ClassMapping
                var mapper = new Nh.Mapping.ByCode.ModelMapper();
                foreach (var mappingAssembly in mappingAssemblies)
                {
                    mapper.AddMappings(Assembly.Load(mappingAssembly).ExportedTypes);
                }
                NhCfg.MappingSchema.HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
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
        _ConfigurationProperties = new(extendedAttributes);
        BuildSessionFactory(configuration);
    }

    protected override IPersistenceContext OnCreateContext()
    {
        ArgumentNullException.ThrowIfNull(_SessionFactory, nameof(_SessionFactory));
        return new NHPersistenceContext(_SessionFactory.OpenSession(), this, _LoggerFactory?.CreateLogger<NHPersistenceContext>());
    }

    #endregion

    #region Public_Methods
    public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext)
        => new NHReadOnlyRepository<T, K>(dataContext);

    public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext)
        => new NHRepository<T, K>(dataContext);
    #endregion
}
