using System;
using System.Collections.Generic;
using System.Text;

namespace ECO.Data
{
    public abstract class PersistenceUnitBase : IPersistenceUnit
    {
        #region Private_Fields

        private string _Name;

        private IList<Type> _Classes = new List<Type>();

        private IList<IPersistenceUnitListener> _Listeners = new List<IPersistenceUnitListener>();

        #endregion

        #region Public_Properties

        public string Name
        {
            get
            {
                return _Name;
            }
        }

        public IEnumerable<Type> Classes
        {
            get { return _Classes; }
        }

        #endregion

        #region Protected_Methods

        protected abstract IPersistenceContext CreateContext();

        protected virtual void OnInitialize(IDictionary<string, string> extendedAttributes)
        {
            
        }

        protected virtual void OnContextPreCreate()
        {
            foreach (IPersistenceUnitListener listener in _Listeners)
            {
                listener.ContextPreCreate(this);
            }
        }

        protected virtual void OnContextPostCreate(IPersistenceContext context)
        {
            foreach (IPersistenceUnitListener listener in _Listeners)
            {
                listener.ContextPostCreate(this, context);
            }
        }

        #endregion

        #region Public_Methods

        public void Initialize(string name, IDictionary<string, string> extededAttributes)
        {
            _Name = name;
            OnInitialize(extededAttributes);
        }

        public virtual IPersistenceContext GetCurrentContext()
        {
            IPersistenceContext context = null;
            if (DataContext.Current.IsContextInitialized(Name))
            {
                context = DataContext.Current[Name];
            }
            else
            {
                OnContextPreCreate();
                context = CreateContext();
                OnContextPostCreate(context);
                DataContext.Current.InitializeContext(Name, context);
            }
            return context;
        }

        public IPersistenceUnit AddClass(Type classType)
        {
            _Classes.Add(classType);
            OnClassAdded(classType);
            return this;
        }

        public IPersistenceUnit AddClass<T, K>() where T : class, IAggregateRoot<K>
        {
            return AddClass(typeof(T));
        }

        public IPersistenceUnit RemoveClass(Type classType)
        {
            _Classes.Remove(classType);
            OnClassRemoved(classType);
            return this;
        }

        public IPersistenceUnit RemoveClass<T, K>() where T : class, IAggregateRoot<K>
        {
            return RemoveClass(typeof(T));
        }

        public IPersistenceUnit AddUnitListener(IPersistenceUnitListener listener)
        {
            _Listeners.Add(listener);
            return this;
        }

        public IPersistenceUnit RemoveUnitListener(IPersistenceUnitListener listener)
        {
            _Listeners.Remove(listener);
            return this;
        }

        public abstract IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>() where T : class, IAggregateRoot<K>;

        public abstract IRepository<T, K> BuildRepository<T, K>() where T : class, IAggregateRoot<K>;

        #endregion

        #region Event_Management

        public event EventHandler<PersistentUnitClassEventArgs> ClassAdded;

        protected virtual void OnClassAdded(Type classType)
        {
            if (ClassAdded != null)
            {
                ClassAdded(this, new PersistentUnitClassEventArgs(this, classType));
            }
        }

        public event EventHandler<PersistentUnitClassEventArgs> ClassRemoved;

        protected virtual void OnClassRemoved(Type classType)
        {
            if (ClassRemoved != null)
            {
                ClassRemoved(this, new PersistentUnitClassEventArgs(this, classType));
            }
        }

        #endregion
    }
}
