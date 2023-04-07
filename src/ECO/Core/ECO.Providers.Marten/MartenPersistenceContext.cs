using ECO.Data;
using Marten;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECO.Providers.Marten
{
    public sealed class MartenPersistenceContext : PersistenceContextBase<MartenPersistenceContext>
    {
        #region Public_Properties

        public IDocumentSession Session { get; private set; }

        #endregion

        #region Ctor

        public MartenPersistenceContext(IDocumentSession session, IPersistenceUnit persistenceUnit, ILogger<MartenPersistenceContext> logger) : base(persistenceUnit, logger)
        {
            Session = session;
        }

        #endregion

        #region Protected_Methods

        #endregion
    }
}
