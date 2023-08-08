using GreenDonut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Integrations.HotChocolate
{
    public class ECOBatchDataLoader<T, K> : BatchDataLoader<K, T>
        where T : class, IAggregateRoot<K>
        where K : IEquatable<K>
    {
        #region Fields

        IReadOnlyRepository<T, K> repository;

        #endregion

        #region Constructors

        public ECOBatchDataLoader(IReadOnlyRepository<T, K> repository, IBatchScheduler batchScheduler, DataLoaderOptions? options = null) : base(batchScheduler, options)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        #endregion

        #region Methods

        protected override async Task<IReadOnlyDictionary<K, T>> LoadBatchAsync(IReadOnlyList<K> keys, CancellationToken cancellationToken) 
            => await Task.FromResult(repository.Where(agg => keys.Contains(agg.Identity)).ToDictionary(agg => agg.Identity, agg => agg));

        #endregion
    }
}
