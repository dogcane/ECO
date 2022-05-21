using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ECO.Bender
{
    [DataContract]
    public struct ErrorMessage : IEquatable<ErrorMessage>
    {
        #region Properties

        [DataMember]
        public string Context { get; private set; }

        [DataMember]
        public string Description { get; private set; }

        #endregion

        #region Ctor

        private ErrorMessage(string context, string description)
            : this()
        {
            Context = context;
            Description = description;
        }
        
        #endregion

        #region Methods

        public static ErrorMessage Create(string description)
        {
            return new ErrorMessage(string.Empty, description);
        }

        public static ErrorMessage Create(string context, string description)
        {
            return new ErrorMessage(context, description);
        }

        internal ErrorMessage AppendContextPrefix(string contextPrefix)
        {
            Context = contextPrefix + Context;
            return this;
        }

        internal ErrorMessage TranslateContext(string newContext)
        {
            Context = newContext;
            return this;
        }

        public override bool Equals(object obj)
        {
            if (obj is ErrorMessage)
            {
                return Equals((ErrorMessage)obj);
            }
            throw new ArgumentException();
        }

        #endregion

        #region IEquatable<ErrorMessage> Membri di

        public bool Equals(ErrorMessage other)
        {
            return Context.Equals(other.Context) && Description.Equals(other.Description);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
