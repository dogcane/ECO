using ECO.Events;
using Xunit;

namespace ECO.DomainEvents.UnitTests;

public class IDomainEventExtensionsTest
{
    public class TestDomainEvent : IDomainEvent
    {
        public string Message { get; set; } = string.Empty;
    }

    [Fact]
    public void Should_raise_event_using_extension_method()
    {
        // Arrange
        var eventRaised = false;
        var testEvent = new TestDomainEvent { Message = "Test" };
        
        EventHandler<DomainEventArgs<IDomainEvent>>? handler = (sender, args) =>
        {
            eventRaised = true;
            Assert.Equivalent(testEvent, args.Value);
        };

        // Act
        ECO.Events.DomainEvents.EventRaised += handler;
        testEvent.Raise(); // Using extension method

        // Assert
        Assert.True(eventRaised);

        // Cleanup
        ECO.Events.DomainEvents.EventRaised -= handler;
    }

    [Fact]
    public void Should_trigger_subscriber_using_extension_method()
    {
        // Arrange
        var eventReceived = false;
        var testEvent = new TestDomainEvent { Message = "Test" };
        var subscriber = new object();
        
        EventAction<TestDomainEvent> callback = (e) =>
        {
            eventReceived = true;
            Assert.Equal("Test", e.Message);
        };

        // Act
        ECO.Events.DomainEvents.Subscribe<TestDomainEvent>(subscriber, callback);
        testEvent.Raise(); // Using extension method

        // Assert
        Assert.True(eventReceived);

        // Cleanup
        ECO.Events.DomainEvents.Unsubscribe<TestDomainEvent>(subscriber);
    }
}