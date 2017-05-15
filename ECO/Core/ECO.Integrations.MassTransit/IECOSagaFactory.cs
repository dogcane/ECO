using MassTransit;
using MassTransit.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Integrations.MassTransit
{
    public interface IECOSagaFactory<TSaga> : ISagaRepository<TSaga>
        where TSaga : class, ISaga, IAggregateRoot<Guid>
    {
        TSaga LoadSaga<TMessage>(ISagaPolicy<TSaga, TMessage> policy, IConsumeContext<TMessage> context, Guid sagaId) where TMessage : class;

        TSaga BuildSaga<TMessage>(ISagaPolicy<TSaga, TMessage> policy, IConsumeContext<TMessage> context, Guid sagaId) where TMessage : class;
    }
}
