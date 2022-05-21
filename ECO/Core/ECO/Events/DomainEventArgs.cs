using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Events
{
    public class DomainEventArgs<T> : EventArgs
    {
        #region Public_Properties

        public T Value { get; protected set; }

        #endregion

        #region Ctor

        public DomainEventArgs(T value)
        {
            Value = value;
        }

        #endregion
    }

    public class DomainEventArgs<T, K> : EventArgs
    {
        #region Public_Properties

        public T FirstValue { get; protected set; }

        public K SecondValue { get; protected set; }

        #endregion

        #region Ctor

        public DomainEventArgs(T firstValue, K secondValue)
        {
            FirstValue = firstValue;
            SecondValue = secondValue;
        }

        #endregion
    }
}
