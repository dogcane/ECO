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
        public bool Success { get; set; }

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

        /// <summary>
        /// Informazioni aggiuntive
        /// </summary>
        [DataMember]
        public string AdditionalInfo { get; set; }

        #endregion

        #region Ctor

        public OperationResult()
        {

        }

        public OperationResult(bool success)
        {
            Success = success;
            Errors = new List<ErrorMessage>();
            Value = default(T);
        }

        public OperationResult(bool success, T value)            
        {
            Success = success;
            Errors = new List<ErrorMessage>();
            Value = value;
        }

        public OperationResult(bool success, IEnumerable<ErrorMessage> errors)
        {
            Success = success & !errors.Any();
            Errors = new List<ErrorMessage>(errors);
            Value = default(T);
        }

        #endregion

        #region Methods

        public static OperationResult<T> MakeSuccess(T value)
        {
            return new OperationResult<T>(true, value);
        }

        public static OperationResult<T> MakeFailure(params ErrorMessage[] errors)
        {
            return new OperationResult<T>(false, errors);
        }

        public static OperationResult<T> MakeFailure(IEnumerable<ErrorMessage> errors)
        {
            return new OperationResult<T>(false, errors);
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
            Success = false;
            Errors.Add(ErrorMessage.Create(context, description));
            return this;
        }

        public OperationResult<T> AppendError(ErrorMessage error)
        {
            Success = false;
            Errors.Add(error);
            return this;
        }

        public OperationResult<T> AppendErrors(IEnumerable<ErrorMessage> errors)
        {
            Success = false;
            Errors.AddRange(errors);
            return this;
        }

        public OperationResult<T> AppendContextPrefix(string contextPrefix)
        {
            for (int i = 0; i < Errors.Count; i++)
            {
                ErrorMessage error = Errors[i];
                Errors[i] = error.AppendContextPrefix(contextPrefix);
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
