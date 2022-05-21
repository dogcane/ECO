using Automatonymous;
using MassTransit.Saga;
using System;

namespace ECO.Integrations.MassTransit
{
    public interface IECOSagaFactory<TSaga> : ISagaRepository<TSaga>, IQuerySagaRepository<TSaga>
        where TSaga : class, SagaStateMachineInstance, IAggregateRoot<Guid>
    {

    }
}
