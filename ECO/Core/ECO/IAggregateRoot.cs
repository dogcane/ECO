using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECO
{
    /// <summary>
    /// Interface that defines an entity who is the root of its aggregate
    /// </summary>
    public interface IAggregateRoot<T> : IEntity<T>
    {

    }
}
