using System;
using Automatonymous;
using ECO.Providers.InMemory;
using MassTransit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECO.Integrations.MassTransit.Tests
{
    [TestClass]
    public class MassTransitSagaUnitTest
    {
        private static IBusControl _Bus;

        private static IRepository<SagaStateTest, Guid> _ECOSagaRepository = new InMemoryRepository<SagaStateTest, Guid>();

        private static ECOSagaRepository<SagaStateTest> _SagaRepository = new ECOSagaRepository<SagaStateTest>(_ECOSagaRepository);

        public static int NumberOfActions = 10000;

        public static volatile bool IsSagaStarted;

        public static volatile bool IsSagaCompleted;

        public static object SagaSyncLock = new object();

        [TestInitialize]
        public void TestSetup()
        {
            _Bus = Bus.Factory.CreateUsingInMemory(cfg =>
            {
                cfg.ReceiveEndpoint("queue_name", ep =>
                {
                    ep.StateMachineSaga(new SagaTest(), _SagaRepository);                    
                });
            });
            _Bus.Start();
        }

        [TestCleanup]
        public void TestTeardown()
        {
            _Bus.Stop();

        }

        [TestMethod]
        public void CreationOfSagaMethod()
        {
            IsSagaStarted = false;
            IsSagaCompleted = false;
            var correlationId = Guid.NewGuid();
            _Bus.Publish(new SagaStarted
            {
                CorrelationId = correlationId
            });
            while (!IsSagaStarted)
            {
                System.Threading.Thread.Sleep(100);
            }
            for(int i = 0; i < NumberOfActions; i++)
            {
                _Bus.Publish(new ActionExecuted
                {
                    CorrelationId = correlationId
                });
            }
            while (!IsSagaCompleted)
            {
                System.Threading.Thread.Sleep(100);
            }
        }
    }

    public class SagaStarted : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }

    public class ActionExecuted : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }

    public class SagaEnded : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }

    public class SagaTest : MassTransitStateMachine<SagaStateTest>
    {
        public State Started { get; private set; }

        public State InProcess { get; private set; }

        public Event<SagaStarted> SagaStarted { get; private set; }

        public Event<ActionExecuted> ActionExecuted { get; private set; }

        public Event<SagaEnded> SagaEnded { get; private set; }

        public SagaTest()
        {
            InstanceState(x => x.CurrentState);

            Event(() => SagaStarted);
            Event(() => ActionExecuted);
            Event(() => SagaEnded);            

            Initially(
                When(SagaStarted)
                    .Then(context =>
                    {
                        lock (MassTransitSagaUnitTest.SagaSyncLock)
                        {
                            context.Instance.NumberOfActions = MassTransitSagaUnitTest.NumberOfActions;
                            MassTransitSagaUnitTest.IsSagaStarted = true;
                        }

                    })
                    .TransitionTo(InProcess)
            );
            During(InProcess,
                When(ActionExecuted)
                    .Then(context =>
                    {
                        lock (MassTransitSagaUnitTest.SagaSyncLock)
                        {
                            context.Instance.NumberOfActionsExecuted++;
                            if (context.Instance.NumberOfActions == context.Instance.NumberOfActionsExecuted)
                            {
                                context.Publish(new SagaEnded
                                {
                                    CorrelationId = context.Instance.CorrelationId
                                });
                            }
                        }
                    }),
                When(SagaEnded).Then(context =>
                {
                    lock (MassTransitSagaUnitTest.SagaSyncLock)
                    {
                        MassTransitSagaUnitTest.IsSagaCompleted = true;
                    }
                }).Finalize()
            );                
            SetCompletedWhenFinalized();
        }
    }

    public class SagaStateTest : AggregateRoot<Guid>, SagaStateMachineInstance
    {
        public string CurrentState { get; set; }

        public Guid CorrelationId
        {
            get { return Identity; }
            set { Identity = value; }
        }

        public int NumberOfActions { get; set; }

        public int NumberOfActionsExecuted { get; set; }
    }
}
