using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Providers.MongoDB
{
    public sealed class MongoIdentityMap
    {
        #region Fields

        private IDictionary<object, object> _Map = new Dictionary<object, object>();

        #endregion

        #region Properties

        public IEnumerable<object> Keys
        {
            get { return _Map.Keys; }
        }

        public object this[object indexer]
        {
            get
            {
                return _Map.ContainsKey(indexer) ? _Map[indexer] : null;
            }
            set
            {
                if (!_Map.ContainsKey(indexer))
                {
                    _Map.Add(indexer, value);
                }
                else
                {
                    _Map[indexer] = value;
                }
            }
        }

        #endregion

        #region Ctor

        public MongoIdentityMap()
        {

        }

        #endregion
    }
}
