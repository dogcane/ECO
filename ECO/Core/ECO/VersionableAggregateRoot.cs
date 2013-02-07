using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECO
{
    [Serializable]
    public abstract class VersionableAggregateRoot<T> : AggregateRoot<T>, IVersionableAggregateRoot<T>
    {
        #region Public_Properties

        public virtual int Version { get; protected set; }

        #endregion

        #region Ctor

        protected VersionableAggregateRoot()
            : base()
        {
            Version = 1;
        }

        protected VersionableAggregateRoot(T id, int version)
            : base(id)
        {
            Version = version;
        }

        #endregion
    }
}
