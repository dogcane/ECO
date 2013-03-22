using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;

using ECO.Data;

namespace ECO.Providers.MongoDB
{
    public class MongoPersistenceUnit : PersistenceUnitBase
    {
        #region Fields

        private MongoDatabase _Database;

        #endregion

        #region Methods

        protected override void OnInitialize(IDictionary<string, string> extededAttributes)
        {
            base.OnInitialize(extededAttributes);
            //DATABASE INITIALIZATION
        }

        protected override IPersistenceContext CreateContext()
        {
            return new MongoPersistenceContext(_Database);
        }

        #endregion
    }
}
