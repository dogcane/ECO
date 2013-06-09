using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;

using ECO.Data;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace ECO.Providers.MongoDB
{
    public class MongoPersistenceUnit : PersistenceUnitBase
    {
        #region Consts

        private static readonly string CONNECTIONSTRING_ATTRIBUTE = "connectionString";

        private static readonly string DATABASE_ATTRIBUTE = "database";

        #endregion

        #region Fields

        private MongoDatabase _MongoDatabase;

        private IDictionary<string, string> _CollectionsByTypes;

        #endregion

        #region Methods

        protected override void OnInitialize(IDictionary<string, string> extendedAttributes)
        {
            base.OnInitialize(extendedAttributes);
            if (!extendedAttributes.ContainsKey(CONNECTIONSTRING_ATTRIBUTE))
            {
                throw new ApplicationException(string.Format("The attribute '{0}' was not found in the persistent unit configuration", CONNECTIONSTRING_ATTRIBUTE));
            }
            if (!extendedAttributes.ContainsKey(DATABASE_ATTRIBUTE))
            {
                throw new ApplicationException(string.Format("The attribute '{0}' was not found in the persistent unit configuration", DATABASE_ATTRIBUTE));
            }
            ConventionPack conventions = new ConventionPack();
            conventions.Add(new ECOMapConvention());
            ConventionRegistry.Register("ECO", conventions, type => type.GetProperty("Identity") != null);
            _MongoDatabase = new MongoClient(extendedAttributes[CONNECTIONSTRING_ATTRIBUTE])
                .GetServer()
                .GetDatabase(extendedAttributes[DATABASE_ATTRIBUTE]);
        }

        protected override IPersistenceContext CreateContext()
        {
            return new MongoPersistenceContext(_MongoDatabase, _CollectionsByTypes);
        }

        #endregion
    }
}
