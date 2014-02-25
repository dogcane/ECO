using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace ECO.WCF
{
    public class DataContextAttribute : Attribute, IParameterInspector, IOperationBehavior
    {
        public bool RequiredTransaction = false;

        public bool AutoCommitTransaction = true;

        #region IParameterInspector Members

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            ECO.Data.DataContext.Current.Close();
        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            ECO.Data.DataContext dtx = new Data.DataContext();
            if (RequiredTransaction)
            {
                dtx.BeginTransaction(AutoCommitTransaction);
            }
            return inputs;
        }

        #endregion

        #region IOperationBehavior Members

        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            clientOperation.ClientParameterInspectors.Add(this);            
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.ParameterInspectors.Add(this);
        }

        public void Validate(OperationDescription operationDescription)
        {
            
        }

        #endregion
    }
}
