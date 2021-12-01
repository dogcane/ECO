using ECO.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using nh = NHibernate;
using nhcfg = NHibernate.Cfg;

namespace ECO.Providers.NHibernate
{
    public sealed class NHPersistenceUnit : PersistenceUnitBase<NHPersistenceUnit>
    {
        #region Consts

        private static readonly string CONNECTIONSTRINNAME_ATTRIBUTE = "connectionStringName";

        private static readonly string DIALECT_ATTRIBUTE = "sqlDialect";

        private static readonly string DRIVER_ATTRIBUTE = "sqlDriver";

        private static readonly string CONNECTIONPROVIDER_ATTRIBUTE = "connectionProvider";

        private static readonly string SESSIONINTERCEPTOR_ATTRIBUTE = "sessionInterceptor";

        private static readonly string MAPPINGASSEMBLIES_ATTRIBUTE = "mappingAssemblies";

        #endregion

        #region Private_Fields

        private string _ConnectionStringName;

        private string _SqlDialect;

        private string _SqlDriver;

        private string _ConnectionProvider;

        private string _InterceptorFullName;

        private string[] _MappingAssemblies;

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
                configuration.DataBaseIntegration(c =>
                {
                    c.ConnectionStringName = _ConnectionStringName;
                    c.Dialect(_SqlDialect);
                    c.Driver(_SqlDriver);
                    if (!string.IsNullOrEmpty(_ConnectionProvider))
                    {
                        c.ConnectionProvider(_ConnectionProvider);
                    }
                    c.KeywordsAutoImport = nhcfg.Hbm2DDLKeyWords.AutoQuote;
                    c.SchemaAction = nhcfg.SchemaAutoAction.Validate;
                    c.LogFormattedSql = true;
                    c.LogSqlInConsole = true;
                });
                if (!string.IsNullOrEmpty(_InterceptorFullName))
                {
                    Type interceptorType = Type.GetType(_InterceptorFullName);
                    nh.IInterceptor interceptor = Activator.CreateInstance(interceptorType) as nh.IInterceptor;
                    configuration.SetInterceptor(interceptor);
                }
                foreach (var mappingAssembly in _MappingAssemblies)
                {
                    configuration.AddAssembly(mappingAssembly);
                }
                _SessionFactory = configuration.BuildSessionFactory();
            }
        }

        #endregion

        #region Protected_Methods

        protected override void OnInitialize(IDictionary<string, string> extendedAttributes)
        {
            base.OnInitialize(extendedAttributes);
            if (extendedAttributes.ContainsKey(CONNECTIONSTRINNAME_ATTRIBUTE))
            {
                _ConnectionStringName = extendedAttributes[CONNECTIONSTRINNAME_ATTRIBUTE];
            }
            else
            {
                throw new ApplicationException($"The attribute '{CONNECTIONSTRINNAME_ATTRIBUTE}' was not found in the persistent unit configuration");
            }
            if (extendedAttributes.ContainsKey(DIALECT_ATTRIBUTE))
            {
                _SqlDialect = extendedAttributes[DIALECT_ATTRIBUTE];
            }
            else
            {
                throw new ApplicationException($"The attribute '{DIALECT_ATTRIBUTE}' was not found in the persistent unit configuration");
            }
            if (extendedAttributes.ContainsKey(DRIVER_ATTRIBUTE))
            {
                _SqlDialect = extendedAttributes[DRIVER_ATTRIBUTE];
            }
            else
            {
                throw new ApplicationException($"The attribute '{DRIVER_ATTRIBUTE}' was not found in the persistent unit configuration");
            }
            if (extendedAttributes.ContainsKey(MAPPINGASSEMBLIES_ATTRIBUTE))
            {
                _MappingAssemblies = extendedAttributes[MAPPINGASSEMBLIES_ATTRIBUTE].Split(";", StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                throw new ApplicationException($"The attribute '{MAPPINGASSEMBLIES_ATTRIBUTE}' was not found in the persistent unit configuration");
            }
            if (extendedAttributes.ContainsKey(CONNECTIONPROVIDER_ATTRIBUTE))
            {
                _ConnectionProvider = extendedAttributes[CONNECTIONPROVIDER_ATTRIBUTE];
            }
            if (extendedAttributes.ContainsKey(SESSIONINTERCEPTOR_ATTRIBUTE))
            {
                _InterceptorFullName = extendedAttributes[SESSIONINTERCEPTOR_ATTRIBUTE];
            }
        }

        protected override IPersistenceContext OnCreateContext()
        {
            BuildSessionFactory();
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

    public static class DbPropertiesConfigurationExtensions
    {
        public static void Dialect(this nhcfg.Loquacious.DbIntegrationConfigurationProperties config, string dialectType)
        {
            MethodInfo method = typeof(nhcfg.Loquacious.DbIntegrationConfigurationProperties).GetType().GetMethod(nameof(nhcfg.Loquacious.DbIntegrationConfigurationProperties.Dialect));
            Type dialect = Type.GetType($"NHibernate.Dialect.{dialectType}, NHibernate");
            MethodInfo generic = method.MakeGenericMethod(dialect);
            generic.Invoke(config, null);
        }

        public static void Driver(this nhcfg.Loquacious.DbIntegrationConfigurationProperties config, string driverType)
        {
            MethodInfo method = typeof(nhcfg.Loquacious.DbIntegrationConfigurationProperties).GetType().GetMethod(nameof(nhcfg.Loquacious.DbIntegrationConfigurationProperties.Driver));
            Type driver = Type.GetType($"NHibernate.Driver.{driverType}, NHibernate");
            MethodInfo generic = method.MakeGenericMethod(driver);
            generic.Invoke(config, null);
        }

        public static void ConnectionProvider(this nhcfg.Loquacious.DbIntegrationConfigurationProperties config, string connectionProviderType)
        {
            MethodInfo method = typeof(nhcfg.Loquacious.DbIntegrationConfigurationProperties).GetType().GetMethod(nameof(nhcfg.Loquacious.DbIntegrationConfigurationProperties.ConnectionProvider));
            Type driver = Type.GetType(connectionProviderType);
            MethodInfo generic = method.MakeGenericMethod(driver);
            generic.Invoke(config, null);
        }
    }
}
