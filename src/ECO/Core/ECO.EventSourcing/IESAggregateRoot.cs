using System;
using System.Collections.Generic;
using System.Text;

namespace ECO.EventSourcing
{
    public interface IESAggregateRoot<T> : IAggregateRoot<T>
    {
        IEnumerable<object> GetUncommittedEvents();

        void ClearUncommittedEvents();
    }
}
