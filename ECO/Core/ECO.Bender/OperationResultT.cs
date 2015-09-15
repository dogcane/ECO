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
    public class OperationResult<T>
    {
        #region Properties

        /// <summary>
        /// Indica se l'operazione ha avuto successo o meno
        /// </summary>        
        [DataMember]
        public bool Success
        {
            get { return Errors.Count == 0; }
        }

        /// <summary>
        /// Valore restituito dall'operazione
        /// </summary>
        [DataMember]
        public T Value { get; set; }

        /// <summary>
        /// Elenco degli errori che si sono verificati durante l'esecuzione dell'operazione
        /// </summary>
        [DataMember]
        public List<ErrorMessage> Errors { get; set; }

        #endregion

        #region Ctor

        public OperationResult()
        {
            Errors = new List<ErrorMessage>();
            Value = default(T);
        }

        public OperationResult(T value)            
        {
            Errors = new List<ErrorMessage>();
            Value = value;
        }

        public OperationResult(IEnumerable<ErrorMessage> errors)
        {
            Errors = new List<ErrorMessage>(errors);
            Value = default(T);
        }

        #endregion

        #region Methods

        public static OperationResult<T> MakeSuccess(T value)
        {
            return new OperationResult<T>(value);
        }

        public static OperationResult<T> MakeFailure(IEnumerable<ErrorMessage> errors)
        {
            return new OperationResult<T>(errors);
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
            return value.Success ? OperationResult.MakeSuccess() : OperationResult.MakeFailure(value.Errors);
        }

        public OperationResult<T> AppendError(string context, string description)
        {
            Errors.Add(ErrorMessage.Create(context, description));
            return this;
        }

        public OperationResult<T> AppendError(ErrorMessage error)
        {
            Errors.Add(error);
            return this;
        }

        public OperationResult<T> AppendErrors(IEnumerable<ErrorMessage> errors)
        {
            Errors.AddRange(errors);
            return this;
        }

        public OperationResult<T> AppendContextPrefix(string contextPrefix)
        {            
            foreach (ErrorMessage error in Errors)
            {
                error.AppendContextPrefix(contextPrefix);
            }
            return this;
        }

        public OperationResult<T> TranslateContext(string oldContext, string newContext)
        {
            for (int i = 0; i < Errors.Count; i++)
            {
                ErrorMessage error = Errors[i];
                if (error.Context.Equals(oldContext, StringComparison.CurrentCultureIgnoreCase))
                {
                    Errors[i] = error.TranslateContext(newContext);
                }
            }
            return this;
        }

        #endregion
    }
}
