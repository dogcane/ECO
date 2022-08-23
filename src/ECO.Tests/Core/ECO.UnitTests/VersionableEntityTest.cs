using ECO.UnitTests.Utils;
using ECO.UnitTests.Utils.Foos;
using Xunit;

namespace ECO.UnitTests
{
    public class VersionableEntityTest
    {
        [Fact]
        public void Should_identity_default_with_empty_constructor()
        {
            var ventity = new VersionableEntityFoo();
            Assert.Equal(default, ventity.Identity);
        }

        [Fact]
        public void Should_version_1_with_empty_constructor()
        {
            var ventity = new VersionableEntityFoo();
            Assert.Equal(1, ventity.Version);
        }

        [Fact]
        public void Should_identity_value_equal_when_constructor_with_identity()
        {
            var ventity = new VersionableEntityFoo(2,2);
            Assert.Equal(2, ventity.Identity);
        }

        [Fact]
        public void Should_version_value_equal_when_constructor_with_version()
        {
            var ventity = new VersionableEntityFoo(2, 2);
            Assert.Equal(2, ventity.Version);
        }
    }
}