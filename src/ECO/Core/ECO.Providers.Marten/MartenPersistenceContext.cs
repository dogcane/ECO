using ECO.Data;
using Marten;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Providers.Marten;

public sealed class MartenPersistenceContext(IDocumentSession session, IPersistenceUnit persistenceUnit, ILogger<MartenPersistenceContext>? logger) : PersistenceContextBase<MartenPersistenceContext>(persistenceUnit, logger)
{
    #region Public_Properties

    public IDocumentSession Session { get; private set; } = session ?? throw new ArgumentNullException(nameof(session));

    #endregion

    #region Protected_Methods

    protected override void OnSaveChanges() => Session.SaveChanges();

    protected override async Task OnSaveChangesAsync(CancellationToken cancellationToken = default) => await Session.SaveChangesAsync(cancellationToken);

    protected override void OnDispose()
    {
        Session?.Dispose();
        Transaction?.Dispose();
    }

    #endregion
}
