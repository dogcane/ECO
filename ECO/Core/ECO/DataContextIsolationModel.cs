using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO
{
    /// <summary>
    /// Type of DataContext instance isolation model
    /// </summary>
    public enum DataContextIsolationModel
    {
        /// <summary>
        /// Shared DataContext
        /// </summary>
        Standard = 0,
        /// <summary>
        /// Isolated DataContext (no enlist in global DataContext)
        /// </summary>
        Isolated = 1
    }
}
