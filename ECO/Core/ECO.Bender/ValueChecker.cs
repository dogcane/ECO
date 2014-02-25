using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECO.Bender
{
    public struct ValueChecker<T>
    {
        #region Properties

        public T Value { get; private set; }

        public OperationResult Result { get; private set; }

        internal string Context { get; private set; }

        internal string Description { get; private set; }

        #endregion

        #region Ctor

        internal ValueChecker(T value, OperationResult result, string context, string description) : this()
        {
            Value = value;
            Result = result;
            Context = context;
            Description = description;
        }

        #endregion

        #region Methods

        internal ValueChecker<T> AppendError(string context, string description)
        {
            Result.AppendError(context ?? Context, description ?? Description);
            return this;
        }

        internal ValueChecker<T> AppendErrors(IEnumerable<ErrorMessage> errors)
        {
            Result.AppendErrors(errors);
            return this;
        }

        public ValueChecker<T> Merge(OperationResult result)
        {
            AppendErrors(result.Errors);
            return this;
        }

        public static ValueChecker<T> For(T value)
        {
            return new ValueChecker<T>(value, OperationResult.MakeSuccess(), string.Empty, string.Empty);
        }

        public static ValueChecker<T> For(T value, OperationResult result)
        {
            return new ValueChecker<T>(value, result, string.Empty, string.Empty);
        }

        public static ValueChecker<T> For(T value, string context)
        {
            return new ValueChecker<T>(value, OperationResult.MakeSuccess(), context, string.Empty);
        }

        public static ValueChecker<T> For(T value, OperationResult result, string context)
        {
            return new ValueChecker<T>(value, result, context, string.Empty);
        }

        public static ValueChecker<T> For(T value, string context, string description)
        {
            return new ValueChecker<T>(value, OperationResult.MakeSuccess(), context, description);
        }

        public static ValueChecker<T> For(T value, OperationResult result, string context, string description)
        {
            return new ValueChecker<T>(value, result, context, description);
        }

        #endregion

        #region Operators

        public static implicit operator OperationResult(ValueChecker<T> checker)
        {
            return checker.Result;
        }

        #endregion
    }
}
