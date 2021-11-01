using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECO
{
    /// <summary>
    /// Class that defines a base for all aggregate's roots
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot<T>
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        protected AggregateRoot()
            : base()
        {

        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="identity"></param>
        protected AggregateRoot(T identity)
            : base(identity)
        {

        }

        #endregion
    }
}
