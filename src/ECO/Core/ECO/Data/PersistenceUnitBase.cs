using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace ECO.Data
{
    public abstract class PersistenceUnitBase<P> : IPersistenceUnit
        where P : PersistenceUnitBase<P>
    {
        #region Private_Fields

        protected string _Name;

        protected IList<Type> _Classes = new List<Type>();

        protected IList<IPersistenceUnitListener> _Listeners = new List<IPersistenceUnitListener>();

        protected readonly ILoggerFactory _LoggerFactory;

        protected readonly ILogger<P> _Logger;

        #endregion

        #region Public_Properties

        public virtual string Name => _Name;

        public virtual IEnumerable<Type> Classes => _Classes;

        public virtual IEnumerable<IPersistenceUnitListener> Listeners => _Listeners;

        #endregion

        #region Ctor

        protected PersistenceUnitBase(ILoggerFactory loggerFactory)
        {
            _LoggerFactory = loggerFactory;
            _Logger = loggerFactory.CreateLogger<P>();
        }

        #endregion

        #region Protected_Methods

        protected abstract IPersistenceContext OnCreateContext();

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
        public virtual void Initialize(string name, IDictionary<string, string> extededAttributes)
        {
            _Name = name;
            OnInitialize(extededAttributes);
        }

        public virtual IPersistenceContext CreateContext()
        {
            OnContextPreCreate();
            IPersistenceContext context = OnCreateContext();
            OnContextPostCreate(context);            
            return context;
        }

        public virtual IPersistenceUnit AddClass(Type classType)
        {
            _Classes.Add(classType);
            return this;
        }

        public virtual IPersistenceUnit AddClass<T, K>() where T : class, IAggregateRoot<K>
        {
            return AddClass(typeof(T));
        }

        public virtual IPersistenceUnit RemoveClass(Type classType)
        {
            _Classes.Remove(classType);
            return this;
        }

        public virtual IPersistenceUnit RemoveClass<T, K>() where T : class, IAggregateRoot<K>
        {
            return RemoveClass(typeof(T));
        }

        public virtual IPersistenceUnit AddUnitListener(IPersistenceUnitListener listener)
        {
            _Listeners.Add(listener);
            return this;
        }

        public virtual IPersistenceUnit RemoveUnitListener(IPersistenceUnitListener listener)
        {
            _Listeners.Remove(listener);
            return this;
        }

        public abstract IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext) where T : class, IAggregateRoot<K>;

        public abstract IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext) where T : class, IAggregateRoot<K>;

        #endregion
    }
}
