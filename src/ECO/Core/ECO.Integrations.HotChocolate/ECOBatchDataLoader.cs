using GreenDonut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Integrations.HotChocolate;

public class ECOBatchDataLoader<T, K>(IReadOnlyRepository<T, K> repository, IBatchScheduler batchScheduler, DataLoaderOptions options) : BatchDataLoader<K, T>(batchScheduler, options)
    where T : class, IAggregateRoot<K>
    where K : notnull, IEquatable<K>
{
    #region Fields

    protected readonly IReadOnlyRepository<T, K> repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion
    #region Constructors

    #endregion

    #region Methods

    protected override async Task<IReadOnlyDictionary<K, T>> LoadBatchAsync(IReadOnlyList<K> keys, CancellationToken cancellationToken)
        => await Task.FromResult(repository.Where(agg => keys.Contains(agg.Identity)).ToDictionary(agg => agg.Identity!, agg => agg));

    #endregion
}
