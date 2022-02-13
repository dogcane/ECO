# ECO

<!---
[![NuGet](http://img.shields.io/nuget/vpre/ECO.svg?label=NuGet)](https://github.com/dogcane/ECO/)
--->

ECO is a library that contains some base classes for DDD-development. It helps to avoid the creation of standard classes like Entity, AggregateRoot or ValueObject and it is based on the concept of "persistent unit".

Each persistent unit can contains different aggregates and ECO will manage the different contexts.

The main components of ECO are:

- Persistence Contexts
- Repositories
- Entity & Aggregates

A simple snipplet:

~~~ c#

using (var dataContext = persistenceUnitFactory.OpenDataContext())
using (var transactionContext = dataContext.BeginTransation())
{
    IEntityRepository myRepository = new EntityRepository(dataContext);
    Entity myEntity = new Entity() { Name = "My Name", Surname = "MySurname" };
    await myRepository.AddAsync(myEntity);
    dataContext.SaveChanges();
    transactionContext.Commit();
}
~~~

Check the [Getting started guide](https://github.com/dogcane/ECO/blob/master/docs/Getting-started.md) or go to the [Summary](https://github.com/dogcane/ECO/blob/master/docs/Summary.md) of the docs.
