# Entities, aggregates and value objects

[These objects](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/microservice-domain-model){:target="_blank"} are the skeleton of every "DDD" software and ECO helps to create your objects without rewriting again the same "old" stuff.

## Entities

ECO allows to define an entity, deriving from the standard class Entity\<T\>:

~~~ c#

public abstract class Entity<T> : IEntity<T>
{
    public virtual T Identity { get; protected set; }
    
    public override bool Equals(...) => ...

    public virtual bool Equals(...)
    {
        ...
    }

    public override int GetHashCode(...) => ...

    #endregion
}
~~~

This standard class implements the IEntity\<T\> interface :

~~~ c#

public interface IEntity<T> : IEquatable<IEntity<T>>
{
    T Identity { get; }
}

~~~

The _\<T\>_ represents the type of the _identity_ of the entity. It's an important attribute that allows to identify this entity into its context and it must be immutable.

## Aggregates

ECO allow to define your _root entity_ or _aggregate_ by deriving from the standard class :

~~~ c#

public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot<T>
{
    ...
}

~~~

This standard class derives from the base class Entity\<T\> and it implements also the IAggregateRoot\<T\> interface :

~~~ c#

public interface IAggregateRoot<T> : IEntity<T>
{

}

~~~

An important thing to remember is that __ECO allows persistence through repositories only for aggregates__. So you have to craft your domain in the right way and you have to pay attention during the definitions of the relations between your objects (aggregates, entities, bounded contexts and so on).
Also an aggregate root should be the only point of ingress for all the entities related to its specific context.

## Value objects

ECO defines a simple interface for value objects :

~~~ c#

public interface IValueObject<T> : IEquatable<T>
{

}

~~~

Go to the next step: [Repositories](https://github.com/dogcane/ECO/blob/master/docs/Repositories.md) or go to the [Summary](https://github.com/dogcane/ECO/blob/master/docs/Summary.md) of the docs.
