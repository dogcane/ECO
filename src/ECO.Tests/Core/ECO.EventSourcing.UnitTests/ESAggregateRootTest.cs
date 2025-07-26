using ECO.EventSourcing;
using Xunit;

namespace ECO.EventSourcing.UnitTests;

public class ESAggregateRootTest
{
    // Test implementations
    public class TestEvent
    {
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class AnotherTestEvent
    {
        public int Value { get; set; }
    }

    public class TestESAggregateRoot : ESAggregateRoot<int>
    {
        public string Name { get; private set; } = string.Empty;
        public int Counter { get; private set; }

        public TestESAggregateRoot() : base()
        {
        }

        public TestESAggregateRoot(int identity) : base(identity)
        {
        }

        public void UpdateName(string name)
        {
            var nameUpdatedEvent = new TestEvent { Message = name };
            OnApply(nameUpdatedEvent, ApplyNameUpdated);
        }

        public void IncrementCounter()
        {
            var incrementEvent = new AnotherTestEvent { Value = 1 };
            OnApply(incrementEvent, ApplyCounterIncremented);
        }

        private void ApplyNameUpdated(TestEvent evt)
        {
            Name = evt.Message;
        }

        private void ApplyCounterIncremented(AnotherTestEvent evt)
        {
            Counter += evt.Value;
        }
    }

    [Fact]
    public void Should_create_esaggregateroot_with_default_constructor()
    {
        // Act
        var aggregate = new TestESAggregateRoot();

        // Assert
        Assert.Equal(default(int), aggregate.Identity);
        Assert.Equal(0, aggregate.Version);
        Assert.Empty(((IESAggregateRoot<int>)aggregate).GetUncommittedEvents());
    }

    [Fact]
    public void Should_create_esaggregateroot_with_identity()
    {
        // Arrange
        var identity = 42;

        // Act
        var aggregate = new TestESAggregateRoot(identity);

        // Assert
        Assert.Equal(identity, aggregate.Identity);
        Assert.Equal(0, aggregate.Version);
        Assert.Empty(((IESAggregateRoot<int>)aggregate).GetUncommittedEvents());
    }

    [Fact]
    public void Should_apply_event_and_increment_version()
    {
        // Arrange
        var aggregate = new TestESAggregateRoot(1);

        // Act
        aggregate.UpdateName("Test Name");

        // Assert
        Assert.Equal("Test Name", aggregate.Name);
        Assert.Equal(1, aggregate.Version);
        Assert.Single(((IESAggregateRoot<int>)aggregate).GetUncommittedEvents());
    }

    [Fact]
    public void Should_apply_multiple_events_and_increment_version()
    {
        // Arrange
        var aggregate = new TestESAggregateRoot(1);

        // Act
        aggregate.UpdateName("Test Name");
        aggregate.IncrementCounter();
        aggregate.IncrementCounter();

        // Assert
        Assert.Equal("Test Name", aggregate.Name);
        Assert.Equal(2, aggregate.Counter);
        Assert.Equal(3, aggregate.Version);
        Assert.Equal(3, ((IESAggregateRoot<int>)aggregate).GetUncommittedEvents().Count());
    }

    [Fact]
    public void Should_track_uncommitted_events()
    {
        // Arrange
        var aggregate = new TestESAggregateRoot(1);

        // Act
        aggregate.UpdateName("Test Name");
        aggregate.IncrementCounter();

        // Assert
        var uncommittedEvents = ((IESAggregateRoot<int>)aggregate).GetUncommittedEvents().ToList();
        Assert.Equal(2, uncommittedEvents.Count);
        Assert.IsType<TestEvent>(uncommittedEvents[0]);
        Assert.IsType<AnotherTestEvent>(uncommittedEvents[1]);
    }

    [Fact]
    public void Should_clear_uncommitted_events()
    {
        // Arrange
        var aggregate = new TestESAggregateRoot(1);
        aggregate.UpdateName("Test Name");
        aggregate.IncrementCounter();

        // Act
        ((IESAggregateRoot<int>)aggregate).ClearUncommittedEvents();

        // Assert
        Assert.Empty(((IESAggregateRoot<int>)aggregate).GetUncommittedEvents());
        // State should remain - only events cleared
        Assert.Equal("Test Name", aggregate.Name);
        Assert.Equal(1, aggregate.Counter);
        Assert.Equal(2, aggregate.Version);
    }

    [Fact]
    public void Should_throw_argumentnullexception_when_event_is_null()
    {
        // Arrange
        var aggregate = new TestESAggregateRoot(1);

        // Act & Assert
        var exception = Assert.Throws<System.Reflection.TargetInvocationException>(() => 
            aggregate.GetType().GetMethod("OnApply", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .MakeGenericMethod(typeof(TestEvent))
                .Invoke(aggregate, new object[] { null!, new Action<TestEvent>(_ => { }) }));
        
        Assert.IsType<ArgumentNullException>(exception.InnerException);
    }

    [Fact]
    public void Should_throw_argumentnullexception_when_apply_handler_is_null()
    {
        // Arrange
        var aggregate = new TestESAggregateRoot(1);
        var testEvent = new TestEvent { Message = "Test" };

        // Act & Assert
        var exception = Assert.Throws<System.Reflection.TargetInvocationException>(() => 
            aggregate.GetType().GetMethod("OnApply", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .MakeGenericMethod(typeof(TestEvent))
                .Invoke(aggregate, new object[] { testEvent, null! }));
        
        Assert.IsType<ArgumentNullException>(exception.InnerException);
    }

    [Fact]
    public void Should_implement_iesaggregateroot_interface()
    {
        // Arrange
        var aggregate = new TestESAggregateRoot(1);

        // Act & Assert
        Assert.IsAssignableFrom<IESAggregateRoot<int>>(aggregate);
        Assert.IsAssignableFrom<IAggregateRoot<int>>(aggregate);
        Assert.IsAssignableFrom<IEntity<int>>(aggregate);
    }

    [Fact]
    public void Should_return_copy_of_uncommitted_events()
    {
        // Arrange
        var aggregate = new TestESAggregateRoot(1);
        aggregate.UpdateName("Test Name");

        // Act
        var firstCall = ((IESAggregateRoot<int>)aggregate).GetUncommittedEvents().ToList();
        var secondCall = ((IESAggregateRoot<int>)aggregate).GetUncommittedEvents().ToList();

        // Assert
        Assert.Equal(firstCall.Count, secondCall.Count);
        Assert.NotSame(firstCall, secondCall); // Different collection instances
    }
}