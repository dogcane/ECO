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
    public struct OperationResult<T>
    {
        #region Private_Fields

        private OperationResult _InnerResult;

        #endregion

        #region Properties

        /// <summary>
        /// Indica se l'operazione ha avuto successo o meno
        /// </summary>
        [DataMember]
        public bool Success
        {
            get { return _InnerResult.Success; }
            private set { }
        }

        /// <summary>
        /// Valore restituito dall'operazione
        /// </summary>
        [DataMember]
        public T Value { get; private set; }

        /// <summary>
        /// Elenco degli errori che si sono verificati durante l'esecuzione dell'operazione
        /// </summary>
        [DataMember]
        public IEnumerable<ErrorMessage> Errors
        {
            get { return _InnerResult.Errors; }
            private set { }
        }

        #endregion

        #region Ctor

        private OperationResult(OperationResult innerResult, T value)
            : this()
        {
            _InnerResult = innerResult;
            Value = value;
        }

        #endregion

        #region Methods

        public static OperationResult<T> MakeSuccess(T value)
        {
            return new OperationResult<T>(OperationResult.MakeSuccess(), value);
        }

        public static OperationResult<T> MakeFailure(IEnumerable<ErrorMessage> errors)
        {
            return new OperationResult<T>(OperationResult.MakeFailure(errors), default(T));
        }

        public static implicit operator OperationResult<T>(T value)
        {
            return OperationResult<T>.MakeSuccess(value);
        }

        public static implicit operator OperationResult<T>(OperationResult result)
        {
            if (result.Success)
            {
                throw new ArgumentException();
            }
            else
            {
                return OperationResult<T>.MakeFailure(result.Errors);
            }
        }

        public static implicit operator OperationResult(OperationResult<T> value)
        {
            return value._InnerResult;
        }

        public OperationResult<T> AppendContextPrefix(string contextPrefix)
        {
            return _InnerResult.AppendContextPrefix(contextPrefix);
        }

        public OperationResult TranslateContext(string oldContext, string newContext)
        {
            return _InnerResult.TranslateContext(oldContext, newContext);
        }

        #endregion
    }
}
