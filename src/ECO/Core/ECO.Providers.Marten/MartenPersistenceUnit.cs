using ECO.Data;
using Marten;
using Marten.Internal.Storage;
using Marten.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECO.Providers.Marten
{
    public sealed class MartenPersistenceUnit : PersistenceUnitBase<MartenPersistenceUnit>
    {
        #region Consts

        private static readonly string CONNECTIONSTRING_ATTRIBUTE = "connectionString";

        private IDocumentStore _DocumentStore;

        #endregion

        #region Ctor

        public MartenPersistenceUnit(string name, ILoggerFactory loggerFactory) : base(name, loggerFactory)
        {

        }

        public MartenPersistenceUnit(string name, ILoggerFactory loggerFactory, IDocumentStore documentStore) : base(name, loggerFactory)
        {
            _DocumentStore = documentStore;            
        }

        #endregion

        #region Private_Methods

        private void BuildDocumentStore()
        {
            if (_DocumentStore == null)
            {
                //Try to build from configuration??
            }
            if (_DocumentStore == null)
            {
                throw new ArgumentNullException();
            }
        }

        #endregion

        #region Protected_Methods

        protected override void OnInitialize(IDictionary<string, string> extendedAttributes)
        {
            base.OnInitialize(extendedAttributes);
            BuildDocumentStore();
        }

        protected override IPersistenceContext OnCreateContext() => new MartenPersistenceContext(_DocumentStore.OpenSession(), this, _LoggerFactory.CreateLogger<MartenPersistenceContext>());

        #endregion

        #region Public_Methods

        public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext) => new MartenReadOnlyRepository<T, K>(dataContext);        

        public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext) => new MartenRepository<T, K>(dataContext);        

        #endregion
    }
}
