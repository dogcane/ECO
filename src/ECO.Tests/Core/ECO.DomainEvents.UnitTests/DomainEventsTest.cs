using ECO.Events;
using Xunit;

namespace ECO.DomainEvents.UnitTests;

public class DomainEventsTest
{
    // Test domain event implementations
    public class TestDomainEvent : IDomainEvent
    {
        public string Message { get; set; } = string.Empty;
    }

    public class AnotherTestDomainEvent : IDomainEvent
    {
        public int Value { get; set; }
    }

    [Fact]
    public void Should_raise_event_and_trigger_global_eventRaised()
    {
        // Arrange
        var eventRaised = false;
        var testEvent = new TestDomainEvent { Message = "Test" };
        
        EventHandler<DomainEventArgs<IDomainEvent>>? handler = (sender, args) =>
        {
            eventRaised = true;
            Assert.Equal(testEvent, args.Value);
        };

        // Act
        ECO.Events.DomainEvents.EventRaised += handler;
        ECO.Events.DomainEvents.RaiseEvent(testEvent);

        // Assert
        Assert.True(eventRaised);

        // Cleanup
        ECO.Events.DomainEvents.EventRaised -= handler;
    }

    [Fact]
    public void Should_subscribe_and_receive_event()
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
        ECO.Events.DomainEvents.RaiseEvent(testEvent);

        // Assert
        Assert.True(eventReceived);

        // Cleanup
        ECO.Events.DomainEvents.Unsubscribe<TestDomainEvent>(subscriber);
    }

    [Fact]
    public void Should_not_receive_event_after_unsubscribe()
    {
        // Arrange
        var eventReceived = false;
        var testEvent = new TestDomainEvent { Message = "Test" };
        var subscriber = new object();
        
        EventAction<TestDomainEvent> callback = (e) => eventReceived = true;

        // Act
        ECO.Events.DomainEvents.Subscribe<TestDomainEvent>(subscriber, callback);
        ECO.Events.DomainEvents.Unsubscribe<TestDomainEvent>(subscriber);
        ECO.Events.DomainEvents.RaiseEvent(testEvent);

        // Assert
        Assert.False(eventReceived);
    }

    [Fact]
    public void Should_support_multiple_subscribers_for_same_event()
    {
        // Arrange
        var firstSubscriberReceived = false;
        var secondSubscriberReceived = false;
        var testEvent = new TestDomainEvent { Message = "Test" };
        var firstSubscriber = new object();
        var secondSubscriber = new object();
        
        EventAction<TestDomainEvent> firstCallback = (e) => firstSubscriberReceived = true;
        EventAction<TestDomainEvent> secondCallback = (e) => secondSubscriberReceived = true;

        // Act
        ECO.Events.DomainEvents.Subscribe<TestDomainEvent>(firstSubscriber, firstCallback);
        ECO.Events.DomainEvents.Subscribe<TestDomainEvent>(secondSubscriber, secondCallback);
        ECO.Events.DomainEvents.RaiseEvent(testEvent);

        // Assert
        Assert.True(firstSubscriberReceived);
        Assert.True(secondSubscriberReceived);

        // Cleanup
        ECO.Events.DomainEvents.Unsubscribe<TestDomainEvent>(firstSubscriber);
        ECO.Events.DomainEvents.Unsubscribe<TestDomainEvent>(secondSubscriber);
    }

    [Fact]
    public void Should_only_trigger_subscribers_for_specific_event_type()
    {
        // Arrange
        var testEventReceived = false;
        var anotherEventReceived = false;
        var testEvent = new TestDomainEvent { Message = "Test" };
        var anotherEvent = new AnotherTestDomainEvent { Value = 42 };
        var subscriber = new object();
        
        EventAction<TestDomainEvent> testCallback = (e) => testEventReceived = true;
        EventAction<AnotherTestDomainEvent> anotherCallback = (e) => anotherEventReceived = true;

        // Act
        ECO.Events.DomainEvents.Subscribe<TestDomainEvent>(subscriber, testCallback);
        ECO.Events.DomainEvents.Subscribe<AnotherTestDomainEvent>(subscriber, anotherCallback);
        
        ECO.Events.DomainEvents.RaiseEvent(testEvent);

        // Assert
        Assert.True(testEventReceived);
        Assert.False(anotherEventReceived);

        // Cleanup
        ECO.Events.DomainEvents.Unsubscribe<TestDomainEvent>(subscriber);
        ECO.Events.DomainEvents.Unsubscribe<AnotherTestDomainEvent>(subscriber);
    }

    [Fact]
    public void Should_trigger_eventManaged_when_raising_event_with_subscribers()
    {
        // Arrange
        var eventManaged = false;
        var testEvent = new TestDomainEvent { Message = "Test" };
        var subscriber = new object();
        
        EventAction<TestDomainEvent> callback = (e) => { };
        
        EventHandler<DomainEventArgs<IDomainEvent, Delegate>>? handler = (sender, args) =>
        {
            eventManaged = true;
            Assert.Equal(testEvent, args.FirstValue);
            Assert.Equal(callback, args.SecondValue);
        };

        // Act
        ECO.Events.DomainEvents.Subscribe<TestDomainEvent>(subscriber, callback);
        ECO.Events.DomainEvents.EventManaged += handler;
        ECO.Events.DomainEvents.RaiseEvent(testEvent);

        // Assert
        Assert.True(eventManaged);

        // Cleanup
        ECO.Events.DomainEvents.EventManaged -= handler;
        ECO.Events.DomainEvents.Unsubscribe<TestDomainEvent>(subscriber);
    }

    [Fact]
    public void Should_throw_argumentnullexception_when_subscriber_is_null()
    {
        // Arrange
        EventAction<TestDomainEvent> callback = (e) => { };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            ECO.Events.DomainEvents.Subscribe<TestDomainEvent>(null!, callback));
    }

    [Fact]
    public void Should_throw_argumentnullexception_when_callback_is_null()
    {
        // Arrange
        var subscriber = new object();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            ECO.Events.DomainEvents.Subscribe<TestDomainEvent>(subscriber, null!));
    }

    [Fact]
    public void Should_throw_argumentnullexception_when_unsubscribing_with_null_subscriber()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            ECO.Events.DomainEvents.Unsubscribe<TestDomainEvent>(null!));
    }

    [Fact]
    public void Should_handle_unsubscribe_for_non_existing_subscriber()
    {
        // Arrange
        var subscriber = new object();

        // Act & Assert - Should not throw
        ECO.Events.DomainEvents.Unsubscribe<TestDomainEvent>(subscriber);
    }

    [Fact]
    public void Should_replace_subscription_when_same_subscriber_subscribes_again()
    {
        // Arrange
        var firstCallbackCalled = false;
        var secondCallbackCalled = false;
        var testEvent = new TestDomainEvent { Message = "Test" };
        var subscriber = new object();
        
        EventAction<TestDomainEvent> firstCallback = (e) => firstCallbackCalled = true;
        EventAction<TestDomainEvent> secondCallback = (e) => secondCallbackCalled = true;

        // Act
        ECO.Events.DomainEvents.Subscribe<TestDomainEvent>(subscriber, firstCallback);
        ECO.Events.DomainEvents.Subscribe<TestDomainEvent>(subscriber, secondCallback); // Replace first subscription
        ECO.Events.DomainEvents.RaiseEvent(testEvent);

        // Assert
        Assert.False(firstCallbackCalled);
        Assert.True(secondCallbackCalled);

        // Cleanup
        ECO.Events.DomainEvents.Unsubscribe<TestDomainEvent>(subscriber);
    }
}