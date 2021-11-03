using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace ECO.Providers.RavenDB
{
    internal static class DocumentStoreFactory
    {
        public static IDocumentStore GetDocumentStore(string serverMode, string connectionStringName)
        {
            if ("embedded".Equals(serverMode, StringComparison.InvariantCultureIgnoreCase))
            {
                return new EmbeddableDocumentStore()
                {
                    DataDirectory = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString,
                    UseEmbeddedHttpServer = true
                };
            }
            else if ("http".Equals(serverMode, StringComparison.InvariantCultureIgnoreCase))
            {
                return new DocumentStore()
                {
                    ConnectionStringName = connectionStringName
                };
            }
            else
            {
                throw new ApplicationException("The server mode is not recognized. Use 'embedded' or 'http'");
            }
        }
    }
}
