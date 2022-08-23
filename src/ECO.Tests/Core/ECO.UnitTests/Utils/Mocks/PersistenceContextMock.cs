using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO.Data;
using ECO.UnitTests.Utils.Foos;
using Moq;

namespace ECO.UnitTests.Utils.Mocks
{
    public class PersistenceContextMock : Mock<IPersistenceContext>
    {
        public DataTransactionMock DataTransaction { get; init; } = new DataTransactionMock();

        public PersistenceContextMock SetupBeginTransactionHandler(Action handler)
        {
            Setup(m => m.BeginTransaction()).Callback(handler).Returns(DataTransaction.Object);
            return this;
        }

        public PersistenceContextMock SetupBeginTransactionAsyncHandler(Action handler)
        {
            Setup(m => m.BeginTransactionAsync(default)).Callback(handler).Returns(Task.FromResult(DataTransaction.Object));
            return this;
        }

        public PersistenceContextMock SetupSaveChangesHandler(Action handler)
        {
            Setup(m => m.SaveChanges()).Callback(handler);
            return this;
        }

        public PersistenceContextMock SetupSaveChangesAsyncHandler(Action handler)
        {
            Setup(m => m.SaveChangesAsync(default)).Callback(handler);
            return this;
        }

        public PersistenceContextMock SetupAttachHandler<T>(IAggregateRoot<T> aggregate, Action handler)
        {
            Setup(m => m.Attach(aggregate)).Callback(handler);
            return this;
        }

        public PersistenceContextMock SetupDetachHandler<T>(IAggregateRoot<T> aggregate, Action handler)
        {
            Setup(m => m.Detach(aggregate)).Callback(handler);
            return this;
        }

        public PersistenceContextMock SetupRefreshHandler<T>(IAggregateRoot<T> aggregate, Action handler)
        {
            Setup(m => m.Refresh(aggregate)).Callback(handler);
            return this;
        }
    }
}
