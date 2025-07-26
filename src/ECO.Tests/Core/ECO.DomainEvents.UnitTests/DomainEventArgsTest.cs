using ECO.Events;
using Xunit;

namespace ECO.DomainEvents.UnitTests;

public class DomainEventArgsTest
{
    [Fact]
    public void Should_construct_single_value_domaineventargs()
    {
        // Arrange
        var value = "test value";

        // Act
        var eventArgs = new DomainEventArgs<string>(value);

        // Assert
        Assert.Equal(value, eventArgs.Value);
    }

    [Fact]
    public void Should_throw_argumentnullexception_when_single_value_is_null()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DomainEventArgs<string>(null!));
    }

    [Fact]
    public void Should_construct_two_value_domaineventargs()
    {
        // Arrange
        var firstValue = "test";
        var secondValue = 42;

        // Act
        var eventArgs = new DomainEventArgs<string, int>(firstValue, secondValue);

        // Assert
        Assert.Equal(firstValue, eventArgs.FirstValue);
        Assert.Equal(secondValue, eventArgs.SecondValue);
    }

    [Fact]
    public void Should_throw_argumentnullexception_when_first_value_is_null_in_two_value()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DomainEventArgs<string, int>(null!, 42));
    }

    [Fact]
    public void Should_throw_argumentnullexception_when_second_value_is_null_in_two_value()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DomainEventArgs<string, string>("test", null!));
    }

    [Fact]
    public void Should_construct_three_value_domaineventargs()
    {
        // Arrange
        var firstValue = "test";
        var secondValue = 42;
        var thirdValue = true;

        // Act
        var eventArgs = new DomainEventArgs<string, int, bool>(firstValue, secondValue, thirdValue);

        // Assert
        Assert.Equal(firstValue, eventArgs.FirstValue);
        Assert.Equal(secondValue, eventArgs.SecondValue);
        Assert.Equal(thirdValue, eventArgs.ThirdValue);
    }

    [Fact]
    public void Should_throw_argumentnullexception_when_first_value_is_null_in_three_value()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DomainEventArgs<string, int, bool>(null!, 42, true));
    }

    [Fact]
    public void Should_throw_argumentnullexception_when_second_value_is_null_in_three_value()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DomainEventArgs<string, string, bool>("test", null!, true));
    }

    [Fact]
    public void Should_throw_argumentnullexception_when_third_value_is_null_in_three_value()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DomainEventArgs<string, int, string>("test", 42, null!));
    }

    [Fact]
    public void Should_construct_four_value_domaineventargs()
    {
        // Arrange
        var firstValue = "test";
        var secondValue = 42;
        var thirdValue = true;
        var fourthValue = 3.14;

        // Act
        var eventArgs = new DomainEventArgs<string, int, bool, double>(firstValue, secondValue, thirdValue, fourthValue);

        // Assert
        Assert.Equal(firstValue, eventArgs.FirstValue);
        Assert.Equal(secondValue, eventArgs.SecondValue);
        Assert.Equal(thirdValue, eventArgs.ThirdValue);
        Assert.Equal(fourthValue, eventArgs.FourthValue);
    }

    [Fact]
    public void Should_throw_argumentnullexception_when_fourth_value_is_null_in_four_value()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DomainEventArgs<string, int, bool, string>("test", 42, true, null!));
    }

    [Fact]
    public void Should_construct_five_value_domaineventargs()
    {
        // Arrange
        var firstValue = "test";
        var secondValue = 42;
        var thirdValue = true;
        var fourthValue = 3.14;
        var fifthValue = 'A';

        // Act
        var eventArgs = new DomainEventArgs<string, int, bool, double, char>(firstValue, secondValue, thirdValue, fourthValue, fifthValue);

        // Assert
        Assert.Equal(firstValue, eventArgs.FirstValue);
        Assert.Equal(secondValue, eventArgs.SecondValue);
        Assert.Equal(thirdValue, eventArgs.ThirdValue);
        Assert.Equal(fourthValue, eventArgs.FourthValue);
        Assert.Equal(fifthValue, eventArgs.FifthValue);
    }

    [Fact]
    public void Should_throw_argumentnullexception_when_fifth_value_is_null_in_five_value()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DomainEventArgs<string, int, bool, double, string>("test", 42, true, 3.14, null!));
    }

    [Fact]
    public void Should_inherit_from_eventargs()
    {
        // Arrange
        var eventArgs = new DomainEventArgs<string>("test");

        // Act & Assert
        Assert.IsAssignableFrom<EventArgs>(eventArgs);
    }
}