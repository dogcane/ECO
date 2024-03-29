﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ECO.Providers.MongoDB
{
    public sealed class MongoIdentityMap
    {
        #region Fields

        private readonly ConcurrentDictionary<object, object> _Map = new ConcurrentDictionary<object, object>();

        #endregion

        #region Properties

        public IEnumerable<object> Keys => _Map.Keys;

        public object this[object indexer]
        {
            get => _Map.ContainsKey(indexer) ? _Map[indexer] : null;
            set => _Map.AddOrUpdate(indexer, value ?? throw new ArgumentNullException(nameof(value)), (@old, @new) => @new);
        }

        #endregion

        #region Methods

        public bool ContainsIdentity(object identity) => _Map.ContainsKey(identity ?? throw new ArgumentNullException(nameof(identity)));

        #endregion
    }
}
