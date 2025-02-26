﻿using ECO.Data;

namespace ECO.EventSourcing;

public interface IESRepository<T, K> : IPersistenceManager<T, K>
    where T : class, IESAggregateRoot<K>
{
    T? Load(K identity);

    Task<T?> LoadAsync(K identity);

    void Save(T item);

    Task SaveAsync(T item);

    IEnumerable<dynamic> LoadEvents(K identity);

    Task<IEnumerable<dynamic>> LoadEventsAsync(K identity);
}
