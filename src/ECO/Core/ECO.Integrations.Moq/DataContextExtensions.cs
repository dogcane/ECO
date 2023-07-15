using ECO.Data;
using Moq;

namespace ECO.Integrations.Moq;

public static class DataContextExtensions
{
    public static Mock<IDataContext> SetupDataContext(this Mock<IDataContext> mock)
    {
        var transactionContext = new Mock<ITransactionContext>();
        transactionContext.Setup(obj => obj.Commit());
        transactionContext.Setup(obj => obj.CommitAsync(CancellationToken.None)).Returns(Task.CompletedTask);
        mock.Setup(obj => obj.BeginTransaction()).Returns(transactionContext.Object);
        mock.Setup(obj => obj.BeginTransactionAsync(CancellationToken.None)).Returns(Task.FromResult(transactionContext.Object));
        mock.Setup(obj => obj.SaveChanges());
        mock.Setup(obj => obj.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
        return mock;
    }
}
