namespace ECO.Data;

/// <summary>
/// Represents the status of a transaction within a <see cref="TransactionContext"/>.
/// </summary>
public enum TransactionStatus
{
    /// <summary>
    /// The transaction is active and has not yet been committed or rolled back.
    /// </summary>
    Alive = 0,

    /// <summary>
    /// The transaction has been successfully committed.
    /// </summary>
    Committed = 1,

    /// <summary>
    /// The transaction has been rolled back.
    /// </summary>
    RolledBack = 2
}

