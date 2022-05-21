using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ECO.Data
{
    public abstract class PersistenceManagerBase<T, K> : IPersistenceManager<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Private_Fields

        private IPersistenceUnit _PersistenceUnit;

        #endregion

        #region Public_Properties

        public virtual IPersistenceUnit PersistenceUnit
        {
            get
            {
                if (_PersistenceUnit == null)
                {
                    _PersistenceUnit = GetPersistenceUnit();
                }
                return _PersistenceUnit;
            }
        }

        #endregion

        #region ~Ctor

        protected PersistenceManagerBase()
        {
            
        }

        #endregion

        #region Private_Methods

        private IPersistenceUnit GetPersistenceUnit()
        {
            return PersistenceUnitFactory.Instance.GetPersistenceUnit(typeof(T));
        }

        #endregion

        #region Public_Methods

        public IPersistenceContext GetCurrentContext()
        {
            if (DataContext.Current == null)
            {
                throw new ApplicationException("The DataContext is closed!");
            }
            return PersistenceUnit.GetCurrentContext();
        }

        #endregion
    }
}
