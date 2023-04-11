using ECO.Data;
using Marten;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        protected override void OnSaveChanges() => Session.SaveChanges();

        protected override async Task OnSaveChangesAsync(CancellationToken cancellationToken = default) => await Session.SaveChangesAsync(cancellationToken);

        #endregion
    }
}
