using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECO.EventSourcing
{
    public abstract class ESAggregateRoot<T> : AggregateRoot<T>, IESAggregateRoot<T>
    {
        #region Fields

        protected IList<IESEvent> _uncommittedEvents = new List<IESEvent>();

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        protected ESAggregateRoot() : base() { }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="identity"></param>
        protected ESAggregateRoot(T identity) : base(identity) { }

        #endregion

        #region IESAggregateRoot Methods
        void IESAggregateRoot<T>.ClearUncommittedEvents() => _uncommittedEvents.Clear();

        IEnumerable<object> IESAggregateRoot<T>.GetUncommittedEvents() => _uncommittedEvents.ToArray();

        #endregion
    }
}
