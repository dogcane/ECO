using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO.Data;
using Magnum.StateMachine;
using MassTransit;
using MassTransit.Exceptions;
using MassTransit.Pipeline;
using MassTransit.Saga;

namespace ECO.Integrations.MassTransit
{
    public class ECOSagaRepository<TSaga> : ISagaRepository<TSaga>
        where TSaga : class, ISaga, IAggregateRoot<Guid>
    {
        #region Private_Fields

        private IRepository<TSaga, Guid> _EntityRepository;

        #endregion
                
        #region Ctor

        public ECOSagaRepository(IRepository<TSaga, Guid> entityRepository)
        {
            _EntityRepository = entityRepository;
        }

        #endregion

        #region ISagaRepository<TInstance> Members

        public IEnumerable<Guid> Find(ISagaFilter<TSaga> filter)
        {
            return Where(filter, x => x.Identity);
        }

        public IEnumerable<Action<IConsumeContext<TMessage>>> GetSaga<TMessage>(IConsumeContext<TMessage> context, Guid sagaId, InstanceHandlerSelector<TSaga, TMessage> selector, ISagaPolicy<TSaga, TMessage> policy) where TMessage : class
        {
            using (DataContext dtx = new DataContext())
            using (TransactionContext tcx = dtx.BeginTransaction(true))
            {
                var instance = _EntityRepository.Where(ent => ent.Identity == sagaId).FirstOrDefault();
                if (instance == null)
                {
                    if (policy.CanCreateInstance(context))
                    {
                        yield return x =>
                        {
                            try
                            {
                                instance = policy.CreateInstance(x, sagaId);

                                foreach (var callback in selector(instance, x))
                                {
                                    callback(x);
                                }

                                if (!policy.CanRemoveInstance(instance))
                                    _EntityRepository.Add(instance);
                            }
                            catch (Exception ex)
                            {
                                var sex = new SagaException("Create Saga Instance Exception", typeof(TSaga),
                                    typeof(TMessage), sagaId, ex);                                

                                if (tcx.Status == TransactionStatus.Alive)
                                    tcx.Rollback();
                                throw sex;
                            }
                        };
                    }
                }
                else
                {
                    if (policy.CanUseExistingInstance(context))
                    {
                        yield return x =>
                        {

                            try
                            {
                                foreach (var callback in selector(instance, x))
                                {
                                    callback(x);
                                }

                                if (policy.CanRemoveInstance(instance))
                                    _EntityRepository.Remove(instance);
                                else
                                    _EntityRepository.Update(instance);
                            }
                            catch (Exception ex)
                            {
                                var sex = new SagaException("Existing Saga Instance Exception", typeof(TSaga),
                                    typeof(TMessage), sagaId, ex);
                                if (tcx.Status == TransactionStatus.Alive)
                                    tcx.Rollback();
                                throw sex;
                            }
                        };
                    }
                }
            }
        }

        public IEnumerable<TResult> Select<TResult>(Func<TSaga, TResult> transformer)
        {
            using (DataContext dtx = new DataContext())
            using (TransactionContext tcx = dtx.BeginTransaction())
            {
                return _EntityRepository.Select(transformer).ToList();
            }
        }

        public IEnumerable<TResult> Where<TResult>(ISagaFilter<TSaga> filter, Func<TSaga, TResult> transformer)
        {
            using (DataContext dtx = new DataContext())
            using (TransactionContext tcx = dtx.BeginTransaction())
            {
                return _EntityRepository.Where(filter.FilterExpression).Select(transformer).ToList();
            }
        }

        public IEnumerable<TSaga> Where(ISagaFilter<TSaga> filter)
        {
            using (DataContext dtx = new DataContext())
            using (TransactionContext tcx = dtx.BeginTransaction())
            {
                return _EntityRepository.Where(filter.FilterExpression).ToList();
            }
        }

        #endregion
    }
}
