using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Raven.Client;
using Raven.Client.Document;

using ECO;
using ECO.Data;

namespace ECO.Providers.RavenDB
{
    public class RavenPersistenUnit : PersistenceUnitBase
    {
        #region Consts

        private static readonly string CONNECTIONSTRINGNAME_ATTRIBUTE = "connectionStringName";

        private static readonly string SERVERMODE_ATTRIBUTE = "serverMode";

        #endregion

        #region Private_Fields

        private string _ConnectionStringName;

        private string _ServerMode;

        private IDocumentStore _DocumentStore;

        #endregion

        #region Protected_Methods

        protected override void OnInitialize(IDictionary<string, string> extendedAttributes)
        {
            base.OnInitialize(extendedAttributes);
            if (extendedAttributes.ContainsKey(CONNECTIONSTRINGNAME_ATTRIBUTE))
            {
                _ConnectionStringName = extendedAttributes[CONNECTIONSTRINGNAME_ATTRIBUTE];
            }
            else
            {
                throw new ApplicationException(string.Format("The attribute '{0}' was not found in the persistent unit configuration", CONNECTIONSTRINGNAME_ATTRIBUTE));
            }
            if (extendedAttributes.ContainsKey(SERVERMODE_ATTRIBUTE))
            {
                _ServerMode = extendedAttributes[SERVERMODE_ATTRIBUTE];
            }
            else
            {
                throw new ApplicationException(string.Format("The attribute '{0}' was not found in the persistent unit configuration", SERVERMODE_ATTRIBUTE));
            }
            _DocumentStore = DocumentStoreFactory.GetDocumentStore(_ServerMode, _ConnectionStringName).Initialize();
        }

        protected override IPersistenceContext CreateContext()
        {
            return new RavenPersistenceContext(_DocumentStore.OpenAsyncSession());
        }        

        public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>()
        {
            return new RavenReadOnlyRepository<T, K>();
        }

        public override IRepository<T, K> BuildRepository<T, K>()
        {
            return new RavenRepository<T, K>();
        }

        #endregion
    }
}
