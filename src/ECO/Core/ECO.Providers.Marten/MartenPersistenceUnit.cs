namespace ECO.Providers.Marten;

using ECO.Data;
using Mrt = global::Marten;
using Microsoft.Extensions.Logging;
using System;

public sealed class MartenPersistenceUnit : PersistenceUnitBase<MartenPersistenceUnit>
{
    #region Consts
    private readonly Mrt.IDocumentStore? _DocumentStore;
    #endregion

    #region Ctor

    public MartenPersistenceUnit(string name, ILoggerFactory? loggerFactory) : base(name, loggerFactory)
    {

    }

    public MartenPersistenceUnit(string name, ILoggerFactory? loggerFactory, Mrt.IDocumentStore documentStore) : base(name, loggerFactory)
    {
        _DocumentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
    }

    #endregion

    #region Protected_Methods

    protected override IPersistenceContext OnCreateContext() => new MartenPersistenceContext((_DocumentStore ?? throw new InvalidOperationException("DocumentStore is not configured")).IdentitySession(), this, _LoggerFactory?.CreateLogger<MartenPersistenceContext>());

    #endregion

    #region Public_Methods

    public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext) => new MartenReadOnlyRepository<T, K>(dataContext ?? throw new ArgumentNullException(nameof(dataContext)));        

    public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext) => new MartenRepository<T, K>(dataContext ?? throw new ArgumentNullException(nameof(dataContext)));        

    #endregion
}
