using ECO.Data;
using ECO.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ECO.UnitTests.Configuration;

public class DataContextOptionsTest
{
    [Theory]
    [InlineData(null)]
    [InlineData(true)]
    [InlineData(false)]
    public void Should_persistenceunitfactoryoptions_be_set_correctly(bool? requireTransaction)
    {
        var action = new Mock<Action<IPersistenceUnitFactory, ILoggerFactory>>();           
        var options = requireTransaction switch
        {
            null => new DataContextOptions
            {
                PersistenceUnitFactoryOptions = action.Object
            },
            _ => new DataContextOptions
            {
                PersistenceUnitFactoryOptions = action.Object,
                RequireTransaction = requireTransaction!.Value
            }

        };
        Assert.Equal(action.Object, options.PersistenceUnitFactoryOptions);
        if (requireTransaction.HasValue)
        {
            Assert.Equal(requireTransaction.Value, options.RequireTransaction);
        }
        else
        {
            Assert.False(options.RequireTransaction);
        }
    }
}