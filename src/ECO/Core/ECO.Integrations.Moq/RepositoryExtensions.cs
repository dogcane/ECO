namespace ECO.Integrations.Moq;

using Moq = global::Moq;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;



public static class RepositoryExtensions
{
    public static Moq.Mock<T> SetupQueryable<T, TItem>(this Moq.Mock<T> queryableMock, IEnumerable<TItem> source)
        where T : class, IEnumerable<TItem>        
    {

        var queryableList = source.AsQueryable();

        queryableMock.As<IQueryable<TItem>>().Setup(x => x.Provider).Returns(queryableList.Provider);
        queryableMock.As<IQueryable<TItem>>().Setup(x => x.Expression).Returns(queryableList.Expression);
        queryableMock.As<IQueryable<TItem>>().Setup(x => x.ElementType).Returns(queryableList.ElementType);
        queryableMock.As<IQueryable<TItem>>().Setup(x => x.GetEnumerator()).Returns(() => queryableList.GetEnumerator());

        return queryableMock;
    }

    public static Moq.Mock<T> SetupRepository<T, TItem, TKey>(this Moq.Mock<T> repository, IList<TItem> source)
        where T : class, IRepository<TItem, TKey>
        where TKey : notnull, IEquatable<TKey>
        where TItem : class, IAggregateRoot<TKey>
    {
        repository.SetupReadOnlyRepository<T, TItem, TKey>(source);
        repository.Setup(obj => obj.Add(Moq.It.IsAny<TItem>())).Callback((TItem item) => {
            source.Add(item);
            item.SetMockedIdentity<TItem, TKey>(source);
        });
        repository.Setup(obj => obj.AddAsync(Moq.It.IsAny<TItem>())).Callback((TItem item) =>
        {
            source.Add(item);
            item.SetMockedIdentity<TItem, TKey>(source);
        }).Returns(Task.CompletedTask);
        repository.Setup(obj => obj.Update(Moq.It.IsAny<TItem>())).Callback((TItem item) => { source.Remove(item); source.Add(item); });
        repository.Setup(obj => obj.UpdateAsync(Moq.It.IsAny<TItem>())).Callback((TItem item) => { source.Remove(item); source.Add(item); }).Returns(Task.CompletedTask);
        repository.Setup(obj => obj.Remove(Moq.It.IsAny<TItem>())).Callback((TItem item) => source.Remove(item));
        repository.Setup(obj => obj.RemoveAsync(Moq.It.IsAny<TItem>())).Callback((TItem item) => source.Remove(item)).Returns(Task.CompletedTask);
        return repository;
    }

    public static Moq.Mock<T> SetupReadOnlyRepository<T, TItem, TKey>(this Moq.Mock<T> repository, IEnumerable<TItem> source)
        where T : class, IReadOnlyRepository<TItem, TKey>
        where TKey : notnull, IEquatable<TKey>
        where TItem : class, IAggregateRoot<TKey>
    {
        repository.SetupQueryable(source);
        repository.Setup(obj => obj.Load(Moq.It.IsAny<TKey>())).Returns((TKey id) => source.FirstOrDefault(obj => obj.Identity!.Equals(id)));
        repository.Setup(obj => obj.LoadAsync(Moq.It.IsAny<TKey>())).Returns((TKey id) => Task.FromResult(source.FirstOrDefault(obj => obj.Identity!.Equals(id)))!);
        return repository;
    }

    private static void SetMockedIdentity<TItem, TKey>(this TItem item, IEnumerable<TItem> source)
        where TItem : class, IAggregateRoot<TKey>
    {
        switch (typeof(TKey))
        {
            case Type t when t == typeof(int) && object.Equals(0, item.Identity):
                item.GetType().GetProperty("Identity", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)!.SetValue(item, (TKey)(object)source.Count());
                break;
            case Type t when t == typeof(long) && object.Equals(0L, item.Identity):
                item.GetType().GetProperty("Identity", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)!.SetValue(item, (TKey)(object)source.LongCount());
                break;
            case Type t when t == typeof(Guid) && object.Equals(Guid.Empty, item.Identity):
                item.GetType().GetProperty("Identity", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)!.SetValue(item, Guid.NewGuid());
                break;
            default:
                break;
        }
    }
}
