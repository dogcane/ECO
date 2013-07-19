﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

using ECO.Data;

namespace ECO.Providers.MongoDB
{
    public class MongoPersistenceUnit : PersistenceUnitBase
    {
        #region Consts

        private static readonly string CONNECTIONSTRING_ATTRIBUTE = "connectionString";

        private static readonly string DATABASE_ATTRIBUTE = "database";

        #endregion

        #region Fields

        private MongoDatabase _Database;

        #endregion

        #region Protected_Methods

        protected virtual void OnSetup(MongoDatabase database)
        {

        }    

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
            ConventionRegistry.Register("ECO-Identity",
                new ConventionPack() { new ECOMapConvention() },
                type => type.GetProperty("Identity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly) != null);
            _Database = new MongoClient(extendedAttributes[CONNECTIONSTRING_ATTRIBUTE])
                .GetServer()
                .GetDatabase(extendedAttributes[DATABASE_ATTRIBUTE]);
            OnSetup(_Database);
        }

        protected override IPersistenceContext CreateContext()
        {
            return new MongoPersistenceContext(_Database);
        }

        #endregion
    }
}
