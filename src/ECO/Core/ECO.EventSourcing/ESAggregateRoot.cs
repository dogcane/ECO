using System;
using System.Collections.Generic;
using System.Linq;

namespace ECO.EventSourcing
{
    public abstract class ESAggregateRoot<T> : AggregateRoot<T>, IESAggregateRoot<T>
    {
        #region Fields

        private readonly IList<object> _uncommittedEvents = new List<object>();

        #endregion

        #region Properties

        public long Version { get; protected set; }

        #endregion

        #region Ctor

        protected ESAggregateRoot() : base() { }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="identity"></param>
        protected ESAggregateRoot(T identity) : base(identity) { }

        #endregion

        #region Protected_Methods

        protected void OnApply<E>(E @event, Action<E> applyHandler)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            if (applyHandler == null) throw new ArgumentNullException(nameof(applyHandler));
            applyHandler(@event);
            _uncommittedEvents.Add(@event);
            Version++;
        }

        #endregion

        #region IESAggregateRoot Methods
        void IESAggregateRoot<T>.ClearUncommittedEvents() => _uncommittedEvents.Clear();

        IEnumerable<object> IESAggregateRoot<T>.GetUncommittedEvents() => _uncommittedEvents.ToArray();

        #endregion
    }
}
