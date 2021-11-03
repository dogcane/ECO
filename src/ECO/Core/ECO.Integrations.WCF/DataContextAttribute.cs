using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Integrations.WCF
{
    public class DataContextOperationInvoker : IOperationInvoker
    {
        #region Fields

        private IOperationInvoker _InnerOperationInvoker;

        private bool _RequiredTransaction;

        private bool _AutoCommitTransaction;

        #endregion

        #region Ctor

        public DataContextOperationInvoker(IOperationInvoker operationInvoker, bool requiredTransaction, bool autoCommitTransaction)
        {
            _InnerOperationInvoker = operationInvoker;
            _RequiredTransaction = requiredTransaction;
            _AutoCommitTransaction = autoCommitTransaction;
        }

        #endregion

        #region IOperationInvoker Members

        public object[] AllocateInputs()
        {
            return _InnerOperationInvoker.AllocateInputs();
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            using (ECO.Data.DataContext dtx = new Data.DataContext())
            {
                if (_RequiredTransaction)
                {
                    dtx.BeginTransaction(_AutoCommitTransaction);
                }
                return _InnerOperationInvoker.Invoke(instance, inputs, out outputs);
            }
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            ECO.Data.DataContext dtx = new Data.DataContext();            
            if (_RequiredTransaction)
            {
                dtx.BeginTransaction(_AutoCommitTransaction);
            }
            return _InnerOperationInvoker.InvokeBegin(instance, inputs, callback, state);
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            object returnValue = _InnerOperationInvoker.InvokeEnd(instance, out outputs, result);
            ECO.Data.DataContext.Current.Close();
            return returnValue;
        }

        public bool IsSynchronous
        {
            get { return _InnerOperationInvoker.IsSynchronous; }
        }

        #endregion
    }

    public class DataContextAttribute : Attribute, IOperationBehavior
    {
        public bool RequiredTransaction = false;

        public bool AutoCommitTransaction = true;

        

        #region IOperationBehavior Members

        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Invoker = new DataContextOperationInvoker(dispatchOperation.Invoker, RequiredTransaction, AutoCommitTransaction);
        }

        public void Validate(OperationDescription operationDescription)
        {
            
        }

        #endregion
    }
}
