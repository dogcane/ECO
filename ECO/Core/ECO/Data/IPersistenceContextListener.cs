using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECO.Data
{
    public interface IPersistenceUnitListener
    {
        #region Methods

        void ContextPreCreate();

        void ContextPostCreate(IPersistenceContext context);

        #endregion
    }
}
