namespace ECO.Events;

/// <summary>
/// Represents event arguments containing a single value.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public class DomainEventArgs<T>(T value) : EventArgs
{
    #region Public_Properties
    /// <summary>
    /// Gets the value associated with the event.
    /// </summary>
    public T Value { get; protected set; } = value ?? throw new ArgumentNullException(nameof(value));
    #endregion
}

/// <summary>
/// Represents event arguments containing two values.
/// </summary>
/// <typeparam name="T">The type of the first value.</typeparam>
/// <typeparam name="K">The type of the second value.</typeparam>
public class DomainEventArgs<T, K>(T firstValue, K secondValue) : EventArgs
{
    #region Public_Properties
    /// <summary>
    /// Gets the first value associated with the event.
    /// </summary>
    public T FirstValue { get; protected set; } = firstValue ?? throw new ArgumentNullException(nameof(firstValue));
    /// <summary>
    /// Gets the second value associated with the event.
    /// </summary>
    public K SecondValue { get; protected set; } = secondValue ?? throw new ArgumentNullException(nameof(secondValue));
    #endregion
}

/// <summary>
/// Represents event arguments containing three values.
/// </summary>
/// <typeparam name="T">The type of the first value.</typeparam>
/// <typeparam name="K">The type of the second value.</typeparam>
/// <typeparam name="L">The type of the third value.</typeparam>
public class DomainEventArgs<T, K, L>(T firstValue, K secondValue, L thirdValue) : EventArgs
{
    #region Public_Properties
    /// <summary>
    /// Gets the first value associated with the event.
    /// </summary>
    public T FirstValue { get; protected set; } = firstValue ?? throw new ArgumentNullException(nameof(firstValue));
    /// <summary>
    /// Gets the second value associated with the event.
    /// </summary>
    public K SecondValue { get; protected set; } = secondValue ?? throw new ArgumentNullException(nameof(secondValue));
    /// <summary>
    /// Gets the third value associated with the event.
    /// </summary>
    public L ThirdValue { get; protected set; } = thirdValue ?? throw new ArgumentNullException(nameof(thirdValue));
    #endregion
}

/// <summary>
/// Represents event arguments containing four values.
/// </summary>
/// <typeparam name="T">The type of the first value.</typeparam>
/// <typeparam name="K">The type of the second value.</typeparam>
/// <typeparam name="L">The type of the third value.</typeparam>
/// <typeparam name="M">The type of the fourth value.</typeparam>
public class DomainEventArgs<T, K, L, M>(T firstValue, K secondValue, L thirdValue, M fourthValue) : EventArgs
{
    #region Public_Properties
    /// <summary>
    /// Gets the first value associated with the event.
    /// </summary>
    public T FirstValue { get; protected set; } = firstValue ?? throw new ArgumentNullException(nameof(firstValue));
    /// <summary>
    /// Gets the second value associated with the event.
    /// </summary>
    public K SecondValue { get; protected set; } = secondValue ?? throw new ArgumentNullException(nameof(secondValue));
    /// <summary>
    /// Gets the third value associated with the event.
    /// </summary>
    public L ThirdValue { get; protected set; } = thirdValue ?? throw new ArgumentNullException(nameof(thirdValue));
    /// <summary>
    /// Gets the fourth value associated with the event.
    /// </summary>
    public M FourthValue { get; protected set; } = fourthValue ?? throw new ArgumentNullException(nameof(fourthValue));
    #endregion
}

/// <summary>
/// Represents event arguments containing five values.
/// </summary>
/// <typeparam name="T">The type of the first value.</typeparam>
/// <typeparam name="K">The type of the second value.</typeparam>
/// <typeparam name="L">The type of the third value.</typeparam>
/// <typeparam name="M">The type of the fourth value.</typeparam>
/// <typeparam name="N">The type of the fifth value.</typeparam>
public class DomainEventArgs<T, K, L, M, N>(T firstValue, K secondValue, L thirdValue, M fourthValue, N fifthValue) : EventArgs
{
    #region Public_Properties
    /// <summary>
    /// Gets the first value associated with the event.
    /// </summary>
    public T FirstValue { get; protected set; } = firstValue ?? throw new ArgumentNullException(nameof(firstValue));
    /// <summary>
    /// Gets the second value associated with the event.
    /// </summary>
    public K SecondValue { get; protected set; } = secondValue ?? throw new ArgumentNullException(nameof(secondValue));
    /// <summary>
    /// Gets the third value associated with the event.
    /// </summary>
    public L ThirdValue { get; protected set; } = thirdValue ?? throw new ArgumentNullException(nameof(thirdValue));
    /// <summary>
    /// Gets the fourth value associated with the event.
    /// </summary>
    public M FourthValue { get; protected set; } = fourthValue ?? throw new ArgumentNullException(nameof(fourthValue));
    /// <summary>
    /// Gets the fifth value associated with the event.
    /// </summary>
    public N FifthValue { get; protected set; } = fifthValue ?? throw new ArgumentNullException(nameof(fifthValue));
    #endregion
}
