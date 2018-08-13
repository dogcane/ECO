using MassTransit;
using MassTransit.Saga;
using Automatonymous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Integrations.MassTransit
{
    public interface IECOSagaFactory<TSaga> : ISagaRepository<TSaga>, IQuerySagaRepository<TSaga>
        where TSaga : class, SagaStateMachineInstance, IAggregateRoot<Guid>
    {

    }
}
