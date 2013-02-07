using System;
using System.Collections.Generic;
using System.Text;

namespace ECO.Data
{
    public interface IPersistenceManager<T, K>
        where T : IAggregateRoot<K>
    {
        #region Properties

        IPersistenceUnit PersistenceUnit { get; }

        #endregion
    }
}
