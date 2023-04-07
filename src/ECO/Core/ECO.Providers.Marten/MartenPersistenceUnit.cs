using ECO.Data;
using Marten;
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

        private static readonly string DATABASE_ATTRIBUTE = "database";

        private static readonly string MAPPINGASSEMBLIES_ATTRIBUTE = "mappingAssemblies";

        private static readonly string ES_USES_ATTRIBUTE = "eventSourcing.useSelfAggregates";

        private IDocumentStore _DocumentStore;

        #endregion

        #region Ctor

        public MartenPersistenceUnit(string name, ILoggerFactory loggerFactory) : base(name, loggerFactory)
        {

        }

        public MartenPersistenceUnit(string name, IDocumentStore documentStore, ILoggerFactory loggerFactory) : base(name, loggerFactory)
        {
            _DocumentStore = documentStore;
        }

        #endregion

        #region Private_Methods

        private void BuildDocumentStore()
        {
            if (_DocumentStore == null)
            {
                //Try to build from configuration
            }
        }

        #endregion

        #region Protected_Methods

        protected override void OnInitialize(IDictionary<string, string> extendedAttributes)
        {
            base.OnInitialize(extendedAttributes);
            BuildDocumentStore();
        }

        #endregion

        #region Public_Methods

        public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext)
        {
            throw new NotImplementedException();
        }

        public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext)
        {
            throw new NotImplementedException();
        }

        protected override IPersistenceContext OnCreateContext()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
