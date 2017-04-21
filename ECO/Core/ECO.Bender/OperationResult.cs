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
    public class OperationResult
    {
        #region Properties

        /// <summary>
        /// Indica se l'operazione ha avuto successo o meno
        /// </summary>        
        [DataMember]
        public bool Success { get; set; }

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
            Success = true;
            Errors = new List<ErrorMessage>();
        }

        public OperationResult(bool success, IEnumerable<ErrorMessage> errors)
        {
            Success = success & !errors.Any();
            Errors = new List<ErrorMessage>(errors);
        }

        #endregion

        #region Methods

        public static OperationResult Begin()
        {
            return OperationResult.MakeSuccess();
        }

        public static OperationResult MakeSuccess()
        {
            return new OperationResult(true);
        }

        public static OperationResult MakeFailure(params ErrorMessage[] errors)
        {
            return new OperationResult(false, errors);
        }

        public static OperationResult MakeFailure(IEnumerable<ErrorMessage> errors)
        {
            return new OperationResult(false, errors);
        }        

        public OperationResult AppendError(string context, string description)
        {
            Success = false;
            Errors.Add(ErrorMessage.Create(context, description));
            return this;
        }

        public OperationResult AppendError(ErrorMessage error)
        {
            Success = false;
            Errors.Add(error);
            return this;
        }

        public OperationResult AppendErrors(IEnumerable<ErrorMessage> errors)
        {
            Success = false;
            Errors.AddRange(errors);
            return this;
        }

        public OperationResult AppendContextPrefix(string contextPrefix)
        {
            for (int i = 0; i < Errors.Count; i++)
            {
                ErrorMessage error = Errors[i];
                Errors[i] = error.AppendContextPrefix(contextPrefix);
            }
            return this;
        }

        public OperationResult TranslateContext(string oldContext, string newContext)
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

        #region Operators

        public static bool operator true(OperationResult o)
        {
            return o.Success;
        }

        public static bool operator false(OperationResult o)
        {
            return !o.Success;
        }
        
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
