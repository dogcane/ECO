using ECO.UnitTests.Utils;
using Xunit;

namespace ECO.UnitTests
{
    public class EntityTest
    {
        [Fact]
        public void Should_identity_default_with_empty_constructor()
        {
            var entity = new EntityFoo();
            Assert.Equal(default(int), entity.Identity);            
        }

        [Fact]
        public void Should_identity_value_equal_when_constructor_with_identity()
        {
            var entity = new EntityFoo(1);
            Assert.Equal(1, entity.Identity);
        }

        [Fact]
        public void Should_entities_equals_with_same_identity()
        {
            var entity1 = new EntityFoo(1);
            var entity2 = new EntityFoo(1);
            Assert.Equal(entity1, entity2);
        }

        [Fact]
        public void Should_entities_not_equals_with_different_identity()
        {
            var entity1 = new EntityFoo(1);
            var entity2 = new EntityFoo(2);
            Assert.NotEqual(entity1, entity2);
        }

        [Fact]
        public void Should_different_kind_entities_not_equals()
        {
            var entity1 = new EntityFoo(1);
            var entity2 = new OtherEntityFoo("1");
            Assert.False(object.Equals(entity1, entity2));
        }

        [Fact]
        public void Should_hashcode_equals_with_same_identity()
        {
            var entity1 = new EntityFoo(1);
            var entity2 = new EntityFoo(1);
            Assert.Equal(entity1.GetHashCode(), entity2.GetHashCode());
        }
    }
}