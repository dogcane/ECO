using ECO.UnitTests.Utils;
using ECO.UnitTests.Utils.Foos;
using System;
using Xunit;

namespace ECO.UnitTests
{
    public class EntityTest
    {
        [Fact]
        public void Should_identity_default_with_empty_constructor()
        {
            var entity = new EntityFoo();
            Assert.Equal(default, entity.Identity);            
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
        public void Should_entity_not_equal_to_null_value()
        {
            var entity1 = new EntityFoo(1);
            Assert.False(object.Equals(entity1, null));
            Assert.False(object.Equals(null, entity1));
            Assert.False(entity1.Equals(null));
        }

        [Fact]
        public void Should_hashcode_equals_with_same_identity()
        {
            var entity1 = new EntityFoo(1);
            var entity2 = new EntityFoo(1);
            Assert.Equal(entity1.GetHashCode(), entity2.GetHashCode());
        }

        [Fact]
        public void Should_hashcode_equals_to_static_combine_method()
        {
            var entity1 = new EntityFoo(1);
            Assert.Equal(entity1.GetHashCode(), HashCode.Combine(1, typeof(EntityFoo)));
        }
    }
}