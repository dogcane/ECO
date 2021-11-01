using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Data
{
    public class PersistentUnitClassEventArgs : EventArgs
    {
        #region Properties

        public IPersistenceUnit PersistenceUnit { get; protected set; }

        public Type ClassType { get; protected set; }

        #endregion

        #region Ctor

        public PersistentUnitClassEventArgs(IPersistenceUnit persistenceUnit, Type classType)
        {
            PersistenceUnit = persistenceUnit;
            ClassType = classType;
        }

        #endregion
    }
}
