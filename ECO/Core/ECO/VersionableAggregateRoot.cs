using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECO
{
    /// <summary>
    /// Class that defines a base for all versionable aggregate's roots
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class VersionableAggregateRoot<T> : AggregateRoot<T>, IVersionableAggregateRoot<T>
    {
        #region Public_Properties

        /// <summary>
        /// Version of the aggregate root
        /// </summary>
        public virtual int Version { get; protected set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        protected VersionableAggregateRoot()
            : base()
        {
            Version = 1;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        protected VersionableAggregateRoot(T id, int version)
            : base(id)
        {
            Version = version;
        }

        #endregion
    }
}
