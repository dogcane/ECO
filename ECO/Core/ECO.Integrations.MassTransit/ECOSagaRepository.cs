using Automatonymous;
using ECO.Data;
using GreenPipes;
using MassTransit;
using MassTransit.Saga;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ECO.Integrations.MassTransit
{
    public class ECOSagaRepository<TSaga> : IECOSagaFactory<TSaga>
        where TSaga : class, SagaStateMachineInstance, ISaga, IAggregateRoot<Guid>
    {
        #region Classes

        /// <summary>
        /// Once the message pipe has processed the saga instance, add it to the saga repository
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        private class MissingPipe<TMessage> : IPipe<SagaConsumeContext<TSaga, TMessage>>
            where TMessage : class
        {
            private readonly IRepository<TSaga, Guid> _EntityRepository;
            private readonly IPipe<SagaConsumeContext<TSaga, TMessage>> _Next;            

            public MissingPipe(IRepository<TSaga, Guid> entityRepository, IPipe<SagaConsumeContext<TSaga, TMessage>> next)
            {
                _EntityRepository = entityRepository;
                _Next = next;
            }

            void IProbeSite.Probe(ProbeContext context)
            {
                _Next.Probe(context);
            }

            public async Task Send(SagaConsumeContext<TSaga, TMessage> context)
            {
                var proxy = new ECOSagaConsumeContext<TSaga, TMessage>(_EntityRepository, context, context.Saga);

                await _Next.Send(proxy).ConfigureAwait(false);

                if (!proxy.IsCompleted)
                    await _EntityRepository.AddAsync(context.Saga);
            }
        }

        #endregion

        #region Private_Fields

        private IRepository<TSaga, Guid> _EntityRepository;

        private IsolationLevel? _IsolationLevel;

        #endregion

        #region Ctor

        public ECOSagaRepository(IRepository<TSaga, Guid> entityRepository)
        {
            _EntityRepository = entityRepository;
        }

        public ECOSagaRepository(IRepository<TSaga, Guid> entityRepository, IsolationLevel isolationLevel)
            :this(entityRepository)
        {
            _IsolationLevel = isolationLevel;
        }

        #endregion

        #region Methods

        private async Task<bool> PreInsertSagaInstance<T>(TSaga instance, ECO.Data.DataContext dtx)
        {
            bool inserted = false;
            try
            {
                await _EntityRepository.AddAsync(instance);
                dtx.SaveChanges();
                inserted = true;
            }
            catch
            {
                //???
            }
            return inserted;
        }

        private Task SendToInstance<T>(SagaQueryConsumeContext<TSaga, T> context, ISagaPolicy<TSaga, T> policy, TSaga instance,
            IPipe<SagaConsumeContext<TSaga, T>> next, ECO.Data.DataContext dtx)
            where T : class
        {
            try
            {
                var sagaConsumeContext = new ECOSagaConsumeContext<TSaga, T>(_EntityRepository, context, instance);

                return policy.Existing(sagaConsumeContext, next);
            }
            catch (SagaException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SagaException(ex.Message, typeof(TSaga), typeof(T), instance.CorrelationId, ex);
            }
        }

        public async Task<IEnumerable<Guid>> Find(ISagaQuery<TSaga> query)
        {
            return await Task.Run(() =>
            {
                using (ECO.Data.DataContext dtx = new DataContext())
                using (ECO.Data.TransactionContext tcx = (_IsolationLevel.HasValue ? dtx.BeginTransaction(_IsolationLevel.Value) : dtx.BeginTransaction()))
                {
                    return _EntityRepository.Where(query.FilterExpression).Select(x => x.CorrelationId);
                }
            });
        }

        void IProbeSite.Probe(ProbeContext context)
        {
            var persistenceUnit = ECO.Data.PersistenceUnitFactory.Instance.GetPersistenceUnit<TSaga>();
            var scope = context.CreateScope("sagaRepository");
            scope.Set(new
            {
                Persistence = persistenceUnit.Name,
                Entities = persistenceUnit.Classes.Select(x => x.GetType().Name).ToArray()
            });
        }

        public async Task Send<T>(ConsumeContext<T> context, ISagaPolicy<TSaga, T> policy, IPipe<SagaConsumeContext<TSaga, T>> next) where T : class
        {
            if (!context.CorrelationId.HasValue)
                throw new SagaException("The CorrelationId was not specified", typeof(TSaga), typeof(T));

            var sagaId = context.CorrelationId.Value;

            using (ECO.Data.DataContext dtx = new DataContext())
            using (ECO.Data.TransactionContext tcx = (_IsolationLevel.HasValue ? dtx.BeginTransaction(true, _IsolationLevel.Value) : dtx.BeginTransaction(true)))
            {
                var inserted = false;

                if (policy.PreInsertInstance(context, out var instance))
                {
                    inserted = await PreInsertSagaInstance<T>(instance, dtx);
                }

                try
                {
                    if (instance == null)
                        instance = await _EntityRepository.LoadAsync(sagaId);
                    if (instance == null)
                    {
                        var missingSagaPipe = new MissingPipe<T>(_EntityRepository, next);
                        await policy.Missing(context, missingSagaPipe).ConfigureAwait(false);
                    }
                    else
                    {
                        var sagaConsumeContext = new ECOSagaConsumeContext<TSaga, T>(_EntityRepository, context, instance);

                        await policy.Existing(sagaConsumeContext, next).ConfigureAwait(false);

                        if (inserted && !sagaConsumeContext.IsCompleted)
                            await _EntityRepository.UpdateAsync(instance);
                    }
                }
                catch (Exception ex)
                {
                    if (tcx != null && tcx.Status == TransactionStatus.Alive)
                        tcx.Rollback();

                    throw;
                }
            }
        }

        public async Task SendQuery<T>(SagaQueryConsumeContext<TSaga, T> context, ISagaPolicy<TSaga, T> policy, IPipe<SagaConsumeContext<TSaga, T>> next) where T : class
        {
            using (ECO.Data.DataContext dtx = new DataContext())
            using (ECO.Data.TransactionContext tcx = (_IsolationLevel.HasValue ? dtx.BeginTransaction(true, _IsolationLevel.Value) : dtx.BeginTransaction(true)))
            {
                try
                {
                    IList<TSaga> instances = _EntityRepository
                        .Where(context.Query.FilterExpression)
                        .ToList();

                    if (instances.Count == 0)
                    {
                        var missingSagaPipe = new MissingPipe<T>(_EntityRepository, next);
                        await policy.Missing(context, missingSagaPipe).ConfigureAwait(false);
                    }
                    else
                        await Task.WhenAll(instances.Select(instance => SendToInstance(context, policy, instance, next, dtx))).ConfigureAwait(false);
                }
                catch (SagaException ex)
                {
                    if (tcx != null && tcx.Status == TransactionStatus.Alive)
                        tcx.Rollback();

                    throw;
                }
                catch (Exception ex)
                {
                    if (tcx != null && tcx.Status == TransactionStatus.Alive)
                        tcx.Rollback();

                    throw new SagaException(ex.Message, typeof(TSaga), typeof(T), Guid.Empty, ex);
                }
            }
        }

        #endregion

    }
}
