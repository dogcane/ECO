using System;
using System.Collections.Generic;
using System.Text;

namespace ECO
{
    [Serializable]
    public abstract class VersionableEntity<T> : Entity<T>, IVersionableEntity<T>
    {
        #region Public_Properties

        public virtual int Version { get; protected set; }

        #endregion

        #region Ctor

        protected VersionableEntity()
            : base()
        {
            Version = 1;
        }

        protected VersionableEntity(T id, int version)
            : base(id)
        {
            Version = version;
        }

        #endregion
    }
}
