using ECO.Data;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ECO.Providers.Marten;

public sealed class MartenPersistenceUnit : PersistenceUnitBase<MartenPersistenceUnit>
{
    #region Consts

    private static readonly string CONNECTIONSTRING_ATTRIBUTE = "connectionString";

    private readonly IDocumentStore? _DocumentStore;

    #endregion

    #region Ctor

    public MartenPersistenceUnit(string name, ILoggerFactory? loggerFactory) : base(name, loggerFactory)
    {

    }

    public MartenPersistenceUnit(string name, ILoggerFactory? loggerFactory, IDocumentStore documentStore) : base(name, loggerFactory)
    {
        _DocumentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
    }

    #endregion

    #region Private_Methods

    private void BuildDocumentStore(IConfiguration configuration)
    {
        if (_DocumentStore == null)
        {
            //Try to build from configuration??
        }
    }

    #endregion

    #region Protected_Methods

    protected override void OnInitialize(IDictionary<string, string> extendedAttributes, IConfiguration configuration)
    {
        base.OnInitialize(extendedAttributes, configuration);
        BuildDocumentStore(configuration);
    }

    protected override IPersistenceContext OnCreateContext() => new MartenPersistenceContext((_DocumentStore ?? throw new NullReferenceException(nameof(_DocumentStore))).OpenSession(), this, _LoggerFactory?.CreateLogger<MartenPersistenceContext>());

    #endregion

    #region Public_Methods

    public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext) => new MartenReadOnlyRepository<T, K>(dataContext ?? throw new ArgumentNullException(nameof(dataContext)));        

    public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext) => new MartenRepository<T, K>(dataContext ?? throw new ArgumentNullException(nameof(dataContext)));        

    #endregion
}
