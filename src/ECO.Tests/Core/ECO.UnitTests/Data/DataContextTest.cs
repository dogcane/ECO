using ECO.Data;
using ECO.UnitTests.Utils.Foos;
using ECO.UnitTests.Utils.Mocks;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ECO.UnitTests.Data;

public class DataContextTest
{
    [Fact]
    public void Should_persistenceunitfactory_managed_with_constructor()
    {
        var persistenceUnitFactory = new Mock<IPersistenceUnitFactory>();
        var dataContext = new DataContext(persistenceUnitFactory.Object);
        var reflectedPersistenceUnitFactory = dataContext.GetType().GetField("_PersistenceUnitFactory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(dataContext);
        Assert.Equal(persistenceUnitFactory.Object, reflectedPersistenceUnitFactory);
    }

    [Fact]
    public void Should_logger_managed_with_constructor()
    {
        var persistenceUnitFactory = new Mock<IPersistenceUnitFactory>();
        var logger = new Mock<ILogger<DataContext>>();
        var dataContext = new DataContext(persistenceUnitFactory.Object, logger.Object);
        var reflectedPersistenceUnitFactory = dataContext.GetType().GetField("_PersistenceUnitFactory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(dataContext);
        Assert.Equal(persistenceUnitFactory.Object, reflectedPersistenceUnitFactory);
        var reflectedLogger = dataContext.GetType().GetField("_Logger", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(dataContext);
        Assert.Equal(logger.Object, reflectedLogger);
    }

    [Fact]
    public void Should_persistenceunitfactory_null_cause_exception_with_constructor()
    {
        Assert.Throws<ArgumentNullException>(() => new DataContext(null!));
    }

    [Fact]
    public void Should_getcurrentcontext_return_correct_persistencecontext_from_type()
    {
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        var dataContext = new DataContext(persistenceUnitFactory.Object);
        var currentContext = dataContext.GetCurrentContext(typeof(AggregateRootFooOfInt));
        Assert.Equal(persistenceUnitFactory.PersistenceUnitMock.PersistenceContextMock.Object, currentContext);
        persistenceUnitFactory.Verify(pu => pu.GetPersistenceUnit(typeof(AggregateRootFooOfInt)), Times.Once);
    }

    [Fact]
    public void Should_getcurrentcontextoft_return_correct_persistencecontext()
    {
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        var dataContext = new DataContext(persistenceUnitFactory.Object);
        var currentContext = dataContext.GetCurrentContext<AggregateRootFooOfInt>();
        Assert.Equal(persistenceUnitFactory.PersistenceUnitMock.PersistenceContextMock.Object, currentContext);
        persistenceUnitFactory.Verify(pu => pu.GetPersistenceUnit(typeof(AggregateRootFooOfInt)), Times.Once);
    }

    [Fact]
    public void Should_getcurrentcontext_return_correct_persistencecontext_from_entity()
    {
        var aggregate = new AggregateRootFooOfInt();
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        var dataContext = new DataContext(persistenceUnitFactory.Object);
        var currentContext = dataContext.GetCurrentContext(aggregate);
        Assert.Equal(persistenceUnitFactory.PersistenceUnitMock.PersistenceContextMock.Object, currentContext);
        persistenceUnitFactory.Verify(pu => pu.GetPersistenceUnit(typeof(AggregateRootFooOfInt)), Times.Once);
    }

    [Fact]
    public void Should_begintransaction_create_new_transactioncontext_with_autocommit_false()
    {
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        var dataContext = new DataContext(persistenceUnitFactory.Object);
        var transactionContext = dataContext.BeginTransaction();
        Assert.Equal(transactionContext, dataContext.Transaction);
        Assert.False(transactionContext.AutoCommit);
    }

    [Fact]
    public void Should_begintransaction_with_autocommit_create_new_transactioncontext_with_selected_value()
    {
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        var firstDataContext = new DataContext(persistenceUnitFactory.Object);
        firstDataContext.BeginTransaction(true);
        Assert.True(firstDataContext.Transaction.AutoCommit);
        var secondDataContext = new DataContext(persistenceUnitFactory.Object);
        secondDataContext.BeginTransaction(false);
        Assert.False(secondDataContext.Transaction.AutoCommit);
    }

    [Fact]
    public void Should_begintransaction_with_already_committed_transaction_create_new_transactioncontext()
    {
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        var dataContext = new DataContext(persistenceUnitFactory.Object);
        var firstTransaction = dataContext.BeginTransaction();
        firstTransaction.Commit();
        var secondTransaction = dataContext.BeginTransaction();
    }

    [Fact]
    public void Should_begintransaction_with_already_active_transaction_throws_exception()
    {
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        var dataContext = new DataContext(persistenceUnitFactory.Object);
        dataContext.BeginTransaction();
        Assert.Throws<InvalidOperationException>(() => dataContext.BeginTransaction());
    }

    [Fact]
    public void Should_begintransaction_called_on_initialized_persistencecontexts()
    {
        var beginTransactionCalled = false;
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        persistenceUnitFactory.PersistenceUnitMock.PersistenceContextMock.SetupBeginTransactionHandler(() => beginTransactionCalled = true);
        var dataContext = new DataContext(persistenceUnitFactory.Object);
        var context = dataContext.GetCurrentContext<AggregateRootFooOfInt>(); //Init Context for AggregateRootFoo
        dataContext.BeginTransaction();
        Assert.True(beginTransactionCalled);
    }

    [Fact]
    public void Should_begintransaction_doesnt_called_on_not_initialized_persistencecontexts()
    {
        var beginTransactionCalled = false;
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        persistenceUnitFactory.PersistenceUnitMock.PersistenceContextMock.SetupBeginTransactionHandler(() => beginTransactionCalled = true);
        var dataContext = new DataContext(persistenceUnitFactory.Object);
        dataContext.BeginTransaction();
        Assert.False(beginTransactionCalled);
    }

    [Fact]
    public void Should_savechanges_called_on_initialized_persistencecontexts()
    {
        var saveChangesCalled = false;
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        persistenceUnitFactory.PersistenceUnitMock.PersistenceContextMock.SetupSaveChangesHandler(() => saveChangesCalled = true);
        var dataContext = new DataContext(persistenceUnitFactory.Object);
        var context = dataContext.GetCurrentContext<AggregateRootFooOfInt>(); //Init Context for AggregateRootFoo
        dataContext.SaveChanges();
        Assert.True(saveChangesCalled);
    }

    [Fact]
    public void Should_savechanges_doesnt_called_on_not_initialized_persistencecontexts()
    {
        var saveChangesCalled = false;
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        persistenceUnitFactory.PersistenceUnitMock.PersistenceContextMock.SetupSaveChangesHandler(() => saveChangesCalled = true);
        var dataContext = new DataContext(persistenceUnitFactory.Object);
        dataContext.SaveChanges();
        Assert.False(saveChangesCalled);
    }

    [Fact]
    public async Task Should_savechangesasync_called_on_initialized_persistencecontext()
    {
        var saveChangesCalled = false;
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        persistenceUnitFactory.PersistenceUnitMock.PersistenceContextMock.SetupSaveChangesAsyncHandler(() => saveChangesCalled = true);
        var dataContext = new DataContext(persistenceUnitFactory.Object);
        var context = dataContext.GetCurrentContext<AggregateRootFooOfInt>(); //Init Context for AggregateRootFoo
        await dataContext.SaveChangesAsync();
        Assert.True(saveChangesCalled);
    }

    [Fact]
    public async Task Should_savechangesasync_doesnt_called_on_uninitialized_persistencecontexts()
    {
        var saveChangesCalled = false;
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        persistenceUnitFactory.PersistenceUnitMock.PersistenceContextMock.SetupSaveChangesAsyncHandler(() => saveChangesCalled = true);
        var dataContext = new DataContext(persistenceUnitFactory.Object);
        await dataContext.SaveChangesAsync();
        Assert.False(saveChangesCalled);
    }

    [Fact]
    public void Should_attach_called_on_persistencecontext()
    {
        var attachCalled = false;
        var aggregate = new AggregateRootFooOfInt();
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        persistenceUnitFactory.PersistenceUnitMock.PersistenceContextMock.SetupAttachHandler(aggregate, () => attachCalled = true);
        var dataContext = new DataContext(persistenceUnitFactory.Object);            
        dataContext.Attach(aggregate);
        Assert.True(attachCalled);
    }

    [Fact]
    public void Should_detach_called_on_persistencecontext()
    {
        var detachCalled = false;
        var aggregate = new AggregateRootFooOfInt();
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        persistenceUnitFactory.PersistenceUnitMock.PersistenceContextMock.SetupDetachHandler(aggregate, () => detachCalled = true);
        var dataContext = new DataContext(persistenceUnitFactory.Object);
        dataContext.Detach(aggregate);
        Assert.True(detachCalled);
    }

    [Fact]
    public void Should_refresh_called_on_persistencecontext()
    {
        var refreshCalled = false;
        var aggregate = new AggregateRootFooOfInt();
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        persistenceUnitFactory.PersistenceUnitMock.PersistenceContextMock.SetupRefreshHandler(aggregate, () => refreshCalled = true);
        var dataContext = new DataContext(persistenceUnitFactory.Object);
        dataContext.Refresh(aggregate);
        Assert.True(refreshCalled);
    }
    [Fact]
    public void Should_persistentestate_obtained_from_persistentcontext()
    {
        var firstAggregate = new AggregateRootFooOfInt(1);
        var secondAggregate = new AggregateRootFooOfInt(2);
        var thirdAggregate = new AggregateRootFooOfInt(3);
        var persistenceUnitFactory = new PersistenceUnitFactoryMock();
        persistenceUnitFactory.PersistenceUnitMock.PersistenceContextMock.Setup(m => m.GetPersistenceState(firstAggregate)).Returns(PersistenceState.Persistent);
        persistenceUnitFactory.PersistenceUnitMock.PersistenceContextMock.Setup(m => m.GetPersistenceState(secondAggregate)).Returns(PersistenceState.Transient);
        persistenceUnitFactory.PersistenceUnitMock.PersistenceContextMock.Setup(m => m.GetPersistenceState(thirdAggregate)).Returns(PersistenceState.Detached);
        var dataContext = new DataContext(persistenceUnitFactory.Object);            
        Assert.Equal(PersistenceState.Persistent, dataContext.GetPersistenceState(firstAggregate));
        Assert.Equal(PersistenceState.Transient, dataContext.GetPersistenceState(secondAggregate));
        Assert.Equal(PersistenceState.Detached, dataContext.GetPersistenceState(thirdAggregate));
    }

}
