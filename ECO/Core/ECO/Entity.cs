using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using ECO.Data;

namespace ECO
{
    /// <summary>
    /// Class that defines a base for all the entities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class Entity<T> : IEntity<T>
    {
        #region Public_Properties

        /// <summary>
        /// Identifier of the entity
        /// </summary>
        public virtual T Identity { get; protected set; }

        #endregion

        #region Ctor

        protected Entity()
            : base()
        {
            Identity = default(T);
        }

        protected Entity(T identity)
            : base()
        {
            Identity = identity;
        }

        #endregion

        #region Public_Methods

        public override bool Equals(object obj)
        {
            return Equals(obj as IEntity<T>);
        }

        public virtual bool Equals(IEntity<T> obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (Identity != null)
            {
                return Identity.Equals(obj.Identity);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Identity.GetHashCode() << 8 & typeof(T).GetHashCode();
        }

        #endregion
    }
}
