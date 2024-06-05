using ECO.UnitTests.Utils;
using ECO.UnitTests.Utils.Foos;
using Xunit;

namespace ECO.UnitTests
{
    public class VersionableAggregateRootTest
    {
        [Fact]
        public void Should_identity_default_with_empty_constructor()
        {
            var entity = new VersionableAggregateRootFooOfInt();
            Assert.Equal(default, entity.Identity);
        }

        [Fact]
        public void Should_version_1_with_empty_constructor()
        {
            var entity = new VersionableAggregateRootFooOfInt();
            Assert.Equal(1, entity.Version);
        }

        [Fact]
        public void Should_identity_value_equal_when_constructor_with_identity()
        {
            var entity = new VersionableAggregateRootFooOfInt(2,2);
            Assert.Equal(2, entity.Identity);
        }

        [Fact]
        public void Should_version_value_equal_when_constructor_with_version()
        {
            var entity = new VersionableAggregateRootFooOfInt(2, 2);
            Assert.Equal(2, entity.Version);
        }
    }
}