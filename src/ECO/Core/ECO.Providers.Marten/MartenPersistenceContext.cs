namespace ECO.Providers.Marten;

using System;
using System.Threading;
using System.Threading.Tasks;
using ECO.Data;
using global::Marten;
using Microsoft.Extensions.Logging;

public sealed class MartenPersistenceContext(IDocumentSession session, IPersistenceUnit persistenceUnit, ILogger<MartenPersistenceContext>? logger) : PersistenceContextBase<MartenPersistenceContext>(persistenceUnit, logger)
{
    #region Public_Properties

    public IDocumentSession Session { get; private set; } = session ?? throw new ArgumentNullException(nameof(session));

    #endregion

    #region Protected_Methods

    protected override void OnSaveChanges() => Session.SaveChangesAsync().RunSynchronously();

    protected override async Task OnSaveChangesAsync(CancellationToken cancellationToken = default) => await Session.SaveChangesAsync(cancellationToken);

    protected override void OnDispose()
    {
        Session?.Dispose();
        Transaction?.Dispose();
    }

    #endregion
}
