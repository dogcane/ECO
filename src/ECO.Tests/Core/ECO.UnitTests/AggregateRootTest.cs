using ECO.UnitTests.Utils.Foos;
using Xunit;

namespace ECO.UnitTests
{
    public class AggregateRootTest
    {
        [Fact]
        public void Should_identity_default_with_empty_constructor()
        {
            var aggregate = new AggregateRootFoo();
            Assert.Equal(default, aggregate.Identity);
        }

        [Fact]
        public void Should_identity_value_equal_when_constructor_with_identity()
        {
            var aggregate = new AggregateRootFoo(1);
            Assert.Equal(1, aggregate.Identity);
        }

    }
}