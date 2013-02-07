using System;
using System.ComponentModel;

namespace ECO.Data
{
    public enum TransactionStatus
    {
        Alive,
        Committed,
        RolledBack
    }
}
