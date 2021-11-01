using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ECO.Data
{
    public interface IECOQueryable<T> : IQueryable<T>, IQueryable, IEnumerable, IEnumerable<T>, IOrderedQueryable, IOrderedQueryable<T>
    {
        #region Methods

        IECOQueryable<T> Include(Expression<Func<T, object>> subSelector);

        #endregion
    }
}
