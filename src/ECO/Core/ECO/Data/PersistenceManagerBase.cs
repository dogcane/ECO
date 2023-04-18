using System;

namespace ECO.Data
{
    public abstract class PersistenceManagerBase<T, K> : IPersistenceManager<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Private_Fields

        private readonly IPersistenceContext _PersistenceContext;

        #endregion

        #region Properties
        public IPersistenceContext PersistenceContext => _PersistenceContext;

        #endregion

        #region ~Ctor

        protected PersistenceManagerBase(IDataContext dataContext)
        {
            if (dataContext == null) throw new ArgumentNullException(nameof(dataContext));
            _PersistenceContext = dataContext.GetCurrentContext(typeof(T));
        }

        #endregion
    }
}
