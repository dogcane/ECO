using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECO.Events
{
    /// <summary>
    /// Class that represent a manager for the domain events
    /// </summary>
    public static class DomainEvents
    {
        #region Private_Fields

        private static IDictionary<Type, IDictionary<object, Delegate>> _Subscribers = new Dictionary<Type, IDictionary<object, Delegate>>();        

        #endregion

        #region Events

        public static event EventHandler<DomainEventArgs<IDomainEvent>> EventRaised;

        public static event EventHandler<DomainEventArgs<IDomainEvent, Delegate>> EventManaged;

        #endregion

        #region Public_Methods

        public static void RaiseEvent<T>(T args) where T : IDomainEvent
        {
            if (EventRaised != null)
            {
                EventRaised(null, new DomainEventArgs<IDomainEvent>(args));
            }
            if (_Subscribers.ContainsKey(typeof(T)))
            {
                _Subscribers[typeof(T)]
                    .Select(it => it.Value)
                    .Cast<EventAction<T>>()
                    .ToList()
                    .ForEach(act =>
                    {
                        if (EventManaged != null)
                        {
                            EventManaged(null, new DomainEventArgs<IDomainEvent, Delegate>(args, act));
                        }
                        act.Invoke(args);
                    });
            }
        }

        public static void Subscribe<T>(object subscriber, EventAction<T> callback) where T : IDomainEvent
        {
            if (!_Subscribers.ContainsKey(typeof(T)))
            {
                _Subscribers.Add(typeof(T), new Dictionary<object, Delegate>());
            }
            if (_Subscribers[typeof(T)].ContainsKey(subscriber))
            {
                _Subscribers[typeof(T)][subscriber] = callback;
            }
            else
            {
                _Subscribers[typeof(T)].Add(subscriber, callback);
            }
        }

        public static void Unsubscribe<T>(object subscriber) where T : IDomainEvent
        {
            if (_Subscribers.ContainsKey(typeof(T)) && _Subscribers[typeof(T)].ContainsKey(subscriber))
            {
                _Subscribers[typeof(T)].Remove(subscriber);
            }
        }

        #endregion
    }
}
