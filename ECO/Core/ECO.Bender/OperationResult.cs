using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ECO.Bender
{
    /// <summary>
    /// Rappresenta il risultato di un operazione
    /// </summary>
    [Serializable]
    [DataContract]
    public struct OperationResult
    {
        #region Fields

        private IList<ErrorMessage> _Errors;

        #endregion

        #region Properties

        /// <summary>
        /// Indica se l'operazione ha avuto successo o meno
        /// </summary>
        [DataMember]
        public bool Success
        {
            get { return _Errors.Count == 0; }
            private set { }
        }

        /// <summary>
        /// Elenco degli errori che si sono verificati durante l'esecuzione dell'operazione
        /// </summary>
        [DataMember]
        public IEnumerable<ErrorMessage> Errors
        {
            get { return _Errors; }
            private set { }
        }        

        #endregion

        #region Ctor

        private OperationResult(IEnumerable<ErrorMessage> errors)
        {
            _Errors = new List<ErrorMessage>(errors);
        }

        #endregion

        #region Methods

        public static OperationResult Begin()
        {
            return OperationResult.MakeSuccess();
        }

        public static OperationResult MakeSuccess()
        {
            return new OperationResult(Enumerable.Empty<ErrorMessage>());
        }

        public static OperationResult MakeFailure(params ErrorMessage[] errors)
        {
            return new OperationResult(errors);
        }

        public static OperationResult MakeFailure(IEnumerable<ErrorMessage> errors)
        {
            return new OperationResult(errors);
        }        

        public OperationResult AppendError(string context, string description)
        {
            _Errors.Add(ErrorMessage.Create(context, description));
            return this;
        }

        public OperationResult AppendError(ErrorMessage error)
        {
            _Errors.Add(error);
            return this;
        }

        public OperationResult AppendErrors(IEnumerable<ErrorMessage> errors)
        {
            foreach (ErrorMessage error in errors)
            {
                _Errors.Add(error);
            }
            return this;
        }

        public OperationResult AppendContextPrefix(string contextPrefix)
        {
            foreach (ErrorMessage error in _Errors)
            {
                error.AppendContextPrefix(contextPrefix);
            }
            return this;
        }

        public OperationResult TranslateContext(string oldContext, string newContext)
        {
            for (int i = 0; i < _Errors.Count; i++)
            {
                ErrorMessage error = _Errors[i];
                if (error.Context.Equals(oldContext, StringComparison.CurrentCultureIgnoreCase))
                {                    
                    _Errors[i] = error.TranslateContext(newContext);
                }
            }
            return this;
        }

        #endregion

        #region Operators

        public static OperationResult operator &(OperationResult o1, OperationResult o2)
        {
            if (o1.Success && o2.Success)
            {
                return o1;
            }
            return OperationResult.MakeFailure(o1.Errors.Concat(o2.Errors));
        }

        #endregion
    }
}
