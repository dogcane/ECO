using Moq;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ECO.Integrations.Moq;

public static class RepositoryExtensions
{
    public static Mock<T> SetupQueryable<T, TItem>(this Mock<T> queryableMock, IEnumerable<TItem> source)
        where T : class, IEnumerable<TItem>
    {

        var queryableList = source.AsQueryable();

        queryableMock.As<IQueryable<TItem>>().Setup(x => x.Provider).Returns(queryableList.Provider);
        queryableMock.As<IQueryable<TItem>>().Setup(x => x.Expression).Returns(queryableList.Expression);
        queryableMock.As<IQueryable<TItem>>().Setup(x => x.ElementType).Returns(queryableList.ElementType);
        queryableMock.As<IQueryable<TItem>>().Setup(x => x.GetEnumerator()).Returns(() => queryableList.GetEnumerator());

        return queryableMock;
    }

    public static Mock<T> SetupRepository<T, TItem, TKey>(this Mock<T> repository, IList<TItem> source)
        where T : class, IRepository<TItem, TKey>
        where TItem : class, IAggregateRoot<TKey>
    {
        repository.SetupReadOnlyRepository<T, TItem, TKey>(source);
        repository.Setup(obj => obj.Add(It.IsAny<TItem>())).Callback((TItem item) => {
            source.Add(item);
            item.SetMockedIdentity<TItem, TKey>(source);
        });
        repository.Setup(obj => obj.AddAsync(It.IsAny<TItem>())).Callback((TItem item) =>
        {
            source.Add(item);
            item.SetMockedIdentity<TItem, TKey>(source);
        }).Returns(Task.CompletedTask);
        repository.Setup(obj => obj.Update(It.IsAny<TItem>())).Callback((TItem item) => { source.Remove(item); source.Add(item); });
        repository.Setup(obj => obj.UpdateAsync(It.IsAny<TItem>())).Callback((TItem item) => { source.Remove(item); source.Add(item); }).Returns(Task.CompletedTask);
        repository.Setup(obj => obj.Remove(It.IsAny<TItem>())).Callback((TItem item) => source.Remove(item));
        repository.Setup(obj => obj.RemoveAsync(It.IsAny<TItem>())).Callback((TItem item) => source.Remove(item)).Returns(Task.CompletedTask);
        return repository;
    }

    public static Mock<T> SetupReadOnlyRepository<T, TItem, TKey>(this Mock<T> repository, IEnumerable<TItem> source)
        where T : class, IReadOnlyRepository<TItem, TKey>
        where TItem : class, IAggregateRoot<TKey>
    {
        repository.SetupQueryable(source);
        repository.Setup(obj => obj.Load(It.IsAny<TKey>())).Returns((TKey id) => source.FirstOrDefault(obj => obj.Identity!.Equals(id)));
        repository.Setup(obj => obj.LoadAsync(It.IsAny<TKey>())).Returns((TKey id) => Task.FromResult(source.FirstOrDefault(obj => obj.Identity!.Equals(id)))!);
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
