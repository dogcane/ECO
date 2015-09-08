using System.Collections.Generic;
using System.ServiceModel;
using ECO.Context;

namespace ECO.Integrations.WCF
{
    public class WcfInstanceContext : IExtension<InstanceContext>
    {
        #region Fields

        private IDictionary<string, object> _Context;

        #endregion

        #region ~Ctor

        public WcfInstanceContext()
        {
            _Context = new Dictionary<string, object>();
        }

        #endregion

        #region IContextProvider Members

        public object GetContextData(string dataKey)
        {
            return _Context.ContainsKey(dataKey) ? _Context[dataKey] : null;
        }

        public void SetContextData(string dataKey, object data)
        {
            _Context[dataKey] = data;
        }

        #endregion

        #region IExtension<InstanceContext> Members

        public void Attach(InstanceContext owner)
        {

        }

        public void Detach(InstanceContext owner)
        {

        }

        #endregion
    }

    public class WcfContextProvider : IContextProvider
    {
        #region ~Ctor

        public WcfContextProvider()
        {
           
        }

        #endregion

        #region Private_Methods

        private void Initialize()
        {
            
        }

        #endregion

        #region Private_Methods

        private WcfInstanceContext GetContext()
        {
            WcfInstanceContext context = OperationContext.Current.InstanceContext.Extensions.Find<WcfInstanceContext>();
            if (context == null)
            {
                context = new WcfInstanceContext();
                OperationContext.Current.InstanceContext.Extensions.Add(context);
            }
            return context;
        }

        #endregion

        #region IContextProvider Members

        public object GetContextData(string dataKey)
        {
            return GetContext().GetContextData(dataKey);
        }

        public void SetContextData(string dataKey, object data)
        {
            GetContext().SetContextData(dataKey, data);
        }

        #endregion
    }
}
