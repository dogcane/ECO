namespace ECO.Providers.NHibernate;

using ECO.Data;
using ECO.Providers.NHibernate.Configuration;
using ECO.Providers.NHibernate.Utils;
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
    private Nh.ISessionFactory? _SessionFactory;
    private NHibernateOptions? _Options;
    #endregion

    #region Internal_Methods

    /// <summary>
    /// Configures the persistence unit with the provided options.
    /// This method is called by the configuration extension.
    /// </summary>
    /// <param name="options">The NHibernate configuration options.</param>
    internal void ConfigureWith(NHibernateOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _Options = options;

        // Add listeners from options
        foreach (var listener in options.Listeners)
        {
            AddUnitListener(listener);
        }

        BuildSessionFactory();
    }

    #endregion

    #region Private_Methods

    private void BuildSessionFactory()
    {
        if (_SessionFactory != null || _Options == null)
            return;

        _SessionFactory = _Options.Configuration.BuildSessionFactory();

        // Auto-register aggregate types found in session factory metadata
        foreach (var classMetadata in _SessionFactory.GetAllClassMetadata())
        {
            var mappedClass = classMetadata.Value.MappedClass;
            if (mappedClass.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAggregateRoot<>)))
                _Classes.Add(mappedClass);
        }
    }

    #endregion

    #region Protected_Methods

    protected override void OnInitialize(IDictionary<string, string> extendedAttributes, IConfiguration configuration)
    {
        base.OnInitialize(extendedAttributes, configuration);
        _Options ??= new NHibernateOptions();

        foreach (var (key, val) in extendedAttributes)
        {
            switch (key)
            {
                case var k when k == SESSIONINTERCEPTOR_ATTRIBUTE:
                    if (Type.GetType(val, false, true) is { } interceptorType &&
                        Activator.CreateInstance(interceptorType) is Nh.IInterceptor interceptor)
                    {
                        _Options.Configuration.SetInterceptor(interceptor);
                    }
                    break;

                case var k when k == MAPPINGASSEMBLIES_ATTRIBUTE:
                    var mappingAssemblies = val.Split(";", StringSplitOptions.RemoveEmptyEntries);
                    foreach (var assemblyName in mappingAssemblies.Where(a => !string.IsNullOrWhiteSpace(a)))
                    {
                        _Options.Configuration.AddAssemblyExtended(assemblyName);
                    }
                    break;

                case var k when val.StartsWith("connectionStringName|", StringComparison.OrdinalIgnoreCase):
                    var connectionStringName = val["connectionStringName|".Length..];
                    var connectionString = configuration.GetConnectionString(connectionStringName);
                    if (string.IsNullOrEmpty(connectionString))
                        throw new ArgumentException($"The connection string {connectionStringName} is not defined in the configuration file");
                    _Options.Configuration.SetProperty(key, connectionString);
                    break;

                default:
                    _Options.Configuration.SetProperty(key, val);
                    break;
            }
        }

        BuildSessionFactory();
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
