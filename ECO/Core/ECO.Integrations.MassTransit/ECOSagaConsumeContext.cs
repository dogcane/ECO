using MassTransit;
using MassTransit.Context;
using MassTransit.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Integrations.MassTransit
{
    public sealed class ECOSagaConsumeContext<TSaga, TMessage> :
        ConsumeContextProxyScope<TMessage>,
        SagaConsumeContext<TSaga, TMessage>
        where TMessage : class
        where TSaga : class, ISaga, IAggregateRoot<Guid>
    {
        #region Properties

        public bool IsCompleted { get; private set; }

        public TSaga Saga { get; }

        public IRepository<TSaga, Guid> EntityRepository { get; private set; }

        Guid? MessageContext.CorrelationId
        {
            get
            {
                return Saga.CorrelationId;
            }
        }

        #endregion

        #region Ctor

        public ECOSagaConsumeContext(IRepository<TSaga, Guid> entityRepository, ConsumeContext<TMessage> context, TSaga instance)
            : base(context)
        {
            Saga = instance;
            EntityRepository = entityRepository;
        }

        #endregion

        #region Methods

        async Task SagaConsumeContext<TSaga>.SetCompleted()
        {
            await EntityRepository.RemoveAsync(Saga);
            IsCompleted = true;
        }

        public SagaConsumeContext<TSaga, T> PopContext<T>() where T : class
        {
            /* ???  => Da definire
            if (!(this is SagaConsumeContext<TSaga, T> context))
                throw new ContextException(string.Format("The ConsumeContext<{0}> could not be cast to {1}", typeof(TMessage).Name, typeof(T).Name);
            return context;
            */
            if (typeof(TMessage) != typeof(T))
                throw new ContextException(string.Format("The ConsumeContext<{0}> could not be cast to {1}", typeof(TMessage).Name, typeof(T).Name));
            return this as SagaConsumeContext<TSaga, T>;
        }

        #endregion
    }
}
