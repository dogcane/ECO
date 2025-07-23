namespace ECO.Integrations.Moq;

using ECO.Data;
using System.Threading;
using System.Threading.Tasks;
using Moq = global::Moq;

public static class DataContextExtensions
{
    public static Moq.Mock<IDataContext> SetupDataContext(this Moq.Mock<IDataContext> mock)
    {
        var transactionContext = new Moq.Mock<ITransactionContext>();
        transactionContext.Setup(obj => obj.Commit());
        transactionContext.Setup(obj => obj.CommitAsync(CancellationToken.None)).Returns(Task.CompletedTask);
        mock.Setup(obj => obj.BeginTransaction()).Returns(transactionContext.Object);
        mock.Setup(obj => obj.BeginTransactionAsync(CancellationToken.None)).Returns(Task.FromResult(transactionContext.Object));
        mock.Setup(obj => obj.SaveChanges());
        mock.Setup(obj => obj.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
        return mock;
    }
}
