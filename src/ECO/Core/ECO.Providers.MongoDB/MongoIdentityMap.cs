namespace ECO.Providers.MongoDB;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public sealed class MongoIdentityMap
{
    #region Fields

    private readonly ConcurrentDictionary<object, object> _Map = new();

    #endregion

    #region Properties

    public IEnumerable<object> Keys => _Map.Keys;

    public object? this[object indexer]
    {
        get => _Map.TryGetValue(indexer, out var value) ? value : null;
        set => _Map.AddOrUpdate(indexer, value ?? throw new ArgumentNullException(nameof(value)), (_, @new) => @new);
    }

    #endregion

    #region Methods

    public bool ContainsIdentity(object identity) => _Map.ContainsKey(identity ?? throw new ArgumentNullException(nameof(identity)));

    #endregion
}
