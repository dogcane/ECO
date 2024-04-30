namespace ECO.Events;

public class DomainEventArgs<T>(T value) : EventArgs
{
    #region Public_Properties
    public T Value { get; protected set; } = value ?? throw new ArgumentNullException(nameof(value));
    #endregion
}

public class DomainEventArgs<T, K>(T firstValue, K secondValue) : EventArgs
{
    #region Public_Properties
    public T FirstValue { get; protected set; } = firstValue ?? throw new ArgumentNullException(nameof(firstValue));
    public K SecondValue { get; protected set; } = secondValue ?? throw new ArgumentNullException(nameof(secondValue));
    #endregion
}

public class DomainEventArgs<T, K, L>(T firstValue, K secondValue, L thirdValue) : EventArgs
{
    #region Public_Properties
    public T FirstValue { get; protected set; } = firstValue ?? throw new ArgumentNullException(nameof(firstValue));
    public K SecondValue { get; protected set; } = secondValue ?? throw new ArgumentNullException(nameof(secondValue));
    public L ThirdValue { get; protected set; } = thirdValue ?? throw new ArgumentNullException(nameof(thirdValue));
    #endregion
}

public class DomainEventArgs<T, K, L, M>(T firstValue, K secondValue, L thirdValue, M fourthValue) : EventArgs
{
    #region Public_Properties
    public T FirstValue { get; protected set; } = firstValue ?? throw new ArgumentNullException(nameof(firstValue));
    public K SecondValue { get; protected set; } = secondValue ?? throw new ArgumentNullException(nameof(secondValue));
    public L ThirdValue { get; protected set; } = thirdValue ?? throw new ArgumentNullException(nameof(thirdValue));
    public M FourthValue { get; protected set; } = fourthValue ?? throw new ArgumentNullException(nameof(fourthValue));
    #endregion
}

public class DomainEventArgs<T, K, L, M, N>(T firstValue, K secondValue, L thirdValue, M fourthValue, N fifthValue) : EventArgs
{
    #region Public_Properties
    public T FirstValue { get; protected set; } = firstValue ?? throw new ArgumentNullException(nameof(firstValue));
    public K SecondValue { get; protected set; } = secondValue ?? throw new ArgumentNullException(nameof(secondValue));
    public L ThirdValue { get; protected set; } = thirdValue ?? throw new ArgumentNullException(nameof(thirdValue));
    public M FourthValue { get; protected set; } = fourthValue ?? throw new ArgumentNullException(nameof(fourthValue));
    public N FifthValue { get; protected set; } = fifthValue ?? throw new ArgumentNullException(nameof(fifthValue));
    #endregion
}
