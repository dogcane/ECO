using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECO.Data
{
    public static class EntityExtensions
    {
        public static PersistenceState GetPersistenceState<TArg0>(this AggregateRoot<TArg0> entity)
        {
            return DataContext.Current.GetPersistenceState<AggregateRoot<TArg0>, TArg0>(entity);            
        }

        public static IPersistenceContext GetPersistenceContext<TArg0>(this AggregateRoot<TArg0> entity)
        {
            return DataContext.Current.GetCurrentContext(entity);
        }
    }
}
