using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

namespace ECO.Bender
{
    /// <summary>
    /// Class that represents the standard rules for validations in the form of extension methods
    /// </summary>
    public static class ValidatorExtensions
    {
        #region With

        public static ValueChecker<T> With<T>(this OperationResult result, T value)
        {
            return new ValueChecker<T>(value, result, null, null);
        }

        public static ValueChecker<T> With<T>(this OperationResult result, T value, string context)
        {
            return new ValueChecker<T>(value, result, context, null);
        }

        public static ValueChecker<T> With<T>(this OperationResult result, T value, string context, string description)
        {
            return new ValueChecker<T>(value, result, context, description);
        }

        public static ValueChecker<T> With<T,K>(this ValueChecker<K> checker, T value)
        {
            return new ValueChecker<T>(value, checker.Result, null, null);
        }

        public static ValueChecker<T> With<T, K>(this ValueChecker<K> checker, T value, string context)
        {
            return new ValueChecker<T>(value, checker.Result, context, null);
        }

        public static ValueChecker<T> With<T, K>(this ValueChecker<K> checker, T value, string context, string description)
        {
            return new ValueChecker<T>(value, checker.Result, context, description);
        }

        #endregion

        #region Required

        public static ValueChecker<T> Required<T>(this ValueChecker<T> checker)
        {
            return Required<T>(checker, checker.Context, checker.Description);
        }

        public static ValueChecker<T> Required<T>(this ValueChecker<T> checker, string message)
        {
            return Required<T>(checker, checker.Context, message);
        }

        public static ValueChecker<T> Required<T>(this ValueChecker<T> checker, string context, string message)            
        {
            if (checker.Value is string && string.IsNullOrEmpty(checker.Value as string))
            {
                checker.AppendError(context, message);
            }
            else if (((object)checker.Value) == null)
            {
                checker.AppendError(context, message);
            }
            return checker;
        }

        #endregion

        #region EqualTo

        public static ValueChecker<T> EqualTo<T>(this ValueChecker<T> checker, T value)
        {
            return EqualTo<T>(checker, value, checker.Context, checker.Description);
        }

        public static ValueChecker<T> EqualTo<T>(this ValueChecker<T> checker, T value, string message)
        {
            return EqualTo<T>(checker, value, checker.Context, message);
        }

        public static ValueChecker<T> EqualTo<T>(this ValueChecker<T> checker, T value, string context, string message)
        {
            if (value != null && !value.Equals(checker.Value))
            {
                checker.AppendError(context, message);
            }
            else if (checker.Value != null && !checker.Value.Equals(value))
            {
                checker.AppendError(context, message);
            }
            return checker;
        }

        #endregion

        #region StringLength

        public static ValueChecker<string> StringLength(this ValueChecker<string> checker, int maxLength)
        {
            return StringLength(checker, maxLength, checker.Context, checker.Description);
        }

        public static ValueChecker<string> StringLength(this ValueChecker<string> checker, int maxLength, string description)
        {
            return StringLength(checker, maxLength, checker.Context, description);
        }

        public static ValueChecker<string> StringLength(this ValueChecker<string> checker, int maxLength, string context, string message)                        
        {
            if (checker.Value != null && checker.Value.Length > maxLength)
            {
                checker.AppendError(context, message);
            }
            return checker;
        }

        #endregion

        #region StringMatch

        public static ValueChecker<string> StringMatch(this ValueChecker<string> checker, string regEx)
        {
            return StringMatch(checker, regEx, checker.Context, checker.Description);
        }

        public static ValueChecker<string> StringMatch(this ValueChecker<string> checker, string regEx, string description)
        {
            return StringMatch(checker, regEx, checker.Context, description);
        }

        public static ValueChecker<string> StringMatch(this ValueChecker<string> checker, string regEx, string context, string message)
        {            
            if (!Regex.IsMatch(checker.Value, regEx))
            {
                checker.AppendError(context, message);
            }
            return checker;
        }

        #endregion

        #region GreaterThen

        public static ValueChecker<byte> GreaterThen(this ValueChecker<byte> checker, byte value)
        {
            return GreaterThen(checker, value, null, null);
        }

        public static ValueChecker<byte> GreaterThen(this ValueChecker<byte> checker, byte value, string description)
        {
            return GreaterThen(checker, value, null, description);
        }

        public static ValueChecker<byte> GreaterThen(this ValueChecker<byte> checker, byte value, string context, string description)
        {
            if (!(checker.Value > value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<int> GreaterThen(this ValueChecker<int> checker, int value)
        {
            return GreaterThen(checker, value, null, null);
        }

        public static ValueChecker<int> GreaterThen(this ValueChecker<int> checker, int value, string description)
        {
            return GreaterThen(checker, value, null, description);
        }

        public static ValueChecker<int> GreaterThen(this ValueChecker<int> checker, int value, string context, string description)
        {
            if (!(checker.Value > value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<long> GreaterThen(this ValueChecker<long> checker, long value)
        {
            return GreaterThen(checker, value, null, null);
        }

        public static ValueChecker<long> GreaterThen(this ValueChecker<long> checker, long value, string description)
        {
            return GreaterThen(checker, value, null, description);
        }

        public static ValueChecker<long> GreaterThen(this ValueChecker<long> checker, long value, string context, string description)
        {
            if (!(checker.Value > value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<float> GreaterThen(this ValueChecker<float> checker, float value)
        {
            return GreaterThen(checker, value, null, null);
        }

        public static ValueChecker<float> GreaterThen(this ValueChecker<float> checker, float value, string description)
        {
            return GreaterThen(checker, value, null, description);
        }

        public static ValueChecker<float> GreaterThen(this ValueChecker<float> checker, float value, string context, string description)
        {
            if (!(checker.Value > value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<double> GreaterThen(this ValueChecker<double> checker, double value)
        {
            return GreaterThen(checker, value, null, null);
        }

        public static ValueChecker<double> GreaterThen(this ValueChecker<double> checker, double value, string description)
        {
            return GreaterThen(checker, value, null, description);
        }

        public static ValueChecker<double> GreaterThen(this ValueChecker<double> checker, double value, string context, string description)
        {
            if (!(checker.Value > value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<decimal> GreaterThen(this ValueChecker<decimal> checker, decimal value)
        {
            return GreaterThen(checker, value, null, null);
        }

        public static ValueChecker<decimal> GreaterThen(this ValueChecker<decimal> checker, decimal value, string description)
        {
            return GreaterThen(checker, value, null, description);
        }

        public static ValueChecker<decimal> GreaterThen(this ValueChecker<decimal> checker, decimal value, string context, string description)
        {
            if (!(checker.Value > value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<DateTime> GreaterThen(this ValueChecker<DateTime> checker, DateTime value)
        {
            return GreaterThen(checker, value, null, null);
        }

        public static ValueChecker<DateTime> GreaterThen(this ValueChecker<DateTime> checker, DateTime value, string description)
        {
            return GreaterThen(checker, value, null, description);
        }

        public static ValueChecker<DateTime> GreaterThen(this ValueChecker<DateTime> checker, DateTime value, string context, string description)
        {
            if (!(checker.Value > value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        #endregion

        #region GreaterThenOrEqual

        public static ValueChecker<byte> GreaterThenOrEqual(this ValueChecker<byte> checker, byte value)
        {
            return GreaterThenOrEqual(checker, value, null, null);
        }

        public static ValueChecker<byte> GreaterThenOrEqual(this ValueChecker<byte> checker, byte value, string description)
        {
            return GreaterThenOrEqual(checker, value, null, description);
        }

        public static ValueChecker<byte> GreaterThenOrEqual(this ValueChecker<byte> checker, byte value, string context, string description)
        {
            if (!(checker.Value >= value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<int> GreaterThenOrEqual(this ValueChecker<int> checker, int value)
        {
            return GreaterThenOrEqual(checker, value, null, null);
        }

        public static ValueChecker<int> GreaterThenOrEqual(this ValueChecker<int> checker, int value, string description)
        {
            return GreaterThenOrEqual(checker, value, null, description);
        }

        public static ValueChecker<int> GreaterThenOrEqual(this ValueChecker<int> checker, int value, string context, string description)
        {
            if (!(checker.Value >= value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<long> GreaterThenOrEqual(this ValueChecker<long> checker, long value)
        {
            return GreaterThenOrEqual(checker, value, null, null);
        }

        public static ValueChecker<long> GreaterThenOrEqual(this ValueChecker<long> checker, long value, string description)
        {
            return GreaterThenOrEqual(checker, value, null, description);
        }

        public static ValueChecker<long> GreaterThenOrEqual(this ValueChecker<long> checker, long value, string context, string description)
        {
            if (!(checker.Value >= value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<float> GreaterThenOrEqual(this ValueChecker<float> checker, float value)
        {
            return GreaterThenOrEqual(checker, value, null, null);
        }

        public static ValueChecker<float> GreaterThenOrEqual(this ValueChecker<float> checker, float value, string description)
        {
            return GreaterThenOrEqual(checker, value, null, description);
        }

        public static ValueChecker<float> GreaterThenOrEqual(this ValueChecker<float> checker, float value, string context, string description)
        {
            if (!(checker.Value >= value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<double> GreaterThenOrEqual(this ValueChecker<double> checker, double value)
        {
            return GreaterThenOrEqual(checker, value, null, null);
        }

        public static ValueChecker<double> GreaterThenOrEqual(this ValueChecker<double> checker, double value, string description)
        {
            return GreaterThenOrEqual(checker, value, null, description);
        }

        public static ValueChecker<double> GreaterThenOrEqual(this ValueChecker<double> checker, double value, string context, string description)
        {
            if (!(checker.Value >= value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<decimal> GreaterThenOrEqual(this ValueChecker<decimal> checker, decimal value)
        {
            return GreaterThenOrEqual(checker, value, null, null);
        }

        public static ValueChecker<decimal> GreaterThenOrEqual(this ValueChecker<decimal> checker, decimal value, string description)
        {
            return GreaterThenOrEqual(checker, value, null, description);
        }

        public static ValueChecker<decimal> GreaterThenOrEqual(this ValueChecker<decimal> checker, decimal value, string context, string description)
        {
            if (!(checker.Value >= value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<DateTime> GreaterThenOrEqual(this ValueChecker<DateTime> checker, DateTime value)
        {
            return GreaterThenOrEqual(checker, value, null, null);
        }

        public static ValueChecker<DateTime> GreaterThenOrEqual(this ValueChecker<DateTime> checker, DateTime value, string description)
        {
            return GreaterThenOrEqual(checker, value, null, description);
        }

        public static ValueChecker<DateTime> GreaterThenOrEqual(this ValueChecker<DateTime> checker, DateTime value, string context, string description)
        {
            if (!(checker.Value >= value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        #endregion

        #region LessThen

        public static ValueChecker<byte> LessThen(this ValueChecker<byte> checker, byte value)
        {
            return LessThen(checker, value, null, null);
        }

        public static ValueChecker<byte> LessThen(this ValueChecker<byte> checker, byte value, string description)
        {
            return LessThen(checker, value, null, description);
        }

        public static ValueChecker<byte> LessThen(this ValueChecker<byte> checker, byte value, string context, string description)
        {
            if (!(checker.Value < value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<int> LessThen(this ValueChecker<int> checker, int value)
        {
            return LessThen(checker, value, null, null);
        }

        public static ValueChecker<int> LessThen(this ValueChecker<int> checker, int value, string description)
        {
            return LessThen(checker, value, null, description);
        }

        public static ValueChecker<int> LessThen(this ValueChecker<int> checker, int value, string context, string description)
        {
            if (!(checker.Value < value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<long> LessThen(this ValueChecker<long> checker, long value)
        {
            return LessThen(checker, value, null, null);
        }

        public static ValueChecker<long> LessThen(this ValueChecker<long> checker, long value, string description)
        {
            return LessThen(checker, value, null, description);
        }

        public static ValueChecker<long> LessThen(this ValueChecker<long> checker, long value, string context, string description)
        {
            if (!(checker.Value < value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<float> LessThen(this ValueChecker<float> checker, float value)
        {
            return LessThen(checker, value, null, null);
        }

        public static ValueChecker<float> LessThen(this ValueChecker<float> checker, float value, string description)
        {
            return LessThen(checker, value, null, description);
        }

        public static ValueChecker<float> LessThen(this ValueChecker<float> checker, float value, string context, string description)
        {
            if (!(checker.Value < value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<double> LessThen(this ValueChecker<double> checker, double value)
        {
            return LessThen(checker, value, null, null);
        }

        public static ValueChecker<double> LessThen(this ValueChecker<double> checker, double value, string description)
        {
            return LessThen(checker, value, null, description);
        }

        public static ValueChecker<double> LessThen(this ValueChecker<double> checker, double value, string context, string description)
        {
            if (!(checker.Value < value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<decimal> LessThen(this ValueChecker<decimal> checker, decimal value)
        {
            return LessThen(checker, value, null, null);
        }

        public static ValueChecker<decimal> LessThen(this ValueChecker<decimal> checker, decimal value, string description)
        {
            return LessThen(checker, value, null, description);
        }

        public static ValueChecker<decimal> LessThen(this ValueChecker<decimal> checker, decimal value, string context, string description)
        {
            if (!(checker.Value < value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<DateTime> LessThen(this ValueChecker<DateTime> checker, DateTime value)
        {
            return LessThen(checker, value, null, null);
        }

        public static ValueChecker<DateTime> LessThen(this ValueChecker<DateTime> checker, DateTime value, string description)
        {
            return LessThen(checker, value, null, description);
        }

        public static ValueChecker<DateTime> LessThen(this ValueChecker<DateTime> checker, DateTime value, string context, string description)
        {
            if (!(checker.Value < value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        #endregion

        #region LessThenOrEqual

        public static ValueChecker<byte> LessThenOrEqual(this ValueChecker<byte> checker, byte value)
        {
            return LessThenOrEqual(checker, value, null, null);
        }

        public static ValueChecker<byte> LessThenOrEqual(this ValueChecker<byte> checker, byte value, string description)
        {
            return LessThenOrEqual(checker, value, null, description);
        }

        public static ValueChecker<byte> LessThenOrEqual(this ValueChecker<byte> checker, byte value, string context, string description)
        {
            if (!(checker.Value <= value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<int> LessThenOrEqual(this ValueChecker<int> checker, int value)
        {
            return LessThenOrEqual(checker, value, null, null);
        }

        public static ValueChecker<int> LessThenOrEqual(this ValueChecker<int> checker, int value, string description)
        {
            return LessThenOrEqual(checker, value, null, description);
        }

        public static ValueChecker<int> LessThenOrEqual(this ValueChecker<int> checker, int value, string context, string description)
        {
            if (!(checker.Value <= value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<long> LessThenOrEqual(this ValueChecker<long> checker, long value)
        {
            return LessThenOrEqual(checker, value, null, null);
        }

        public static ValueChecker<long> LessThenOrEqual(this ValueChecker<long> checker, long value, string description)
        {
            return LessThenOrEqual(checker, value, null, description);
        }

        public static ValueChecker<long> LessThenOrEqual(this ValueChecker<long> checker, long value, string context, string description)
        {
            if (!(checker.Value <= value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<float> LessThenOrEqual(this ValueChecker<float> checker, float value)
        {
            return LessThenOrEqual(checker, value, null, null);
        }

        public static ValueChecker<float> LessThenOrEqual(this ValueChecker<float> checker, float value, string description)
        {
            return LessThenOrEqual(checker, value, null, description);
        }

        public static ValueChecker<float> LessThenOrEqual(this ValueChecker<float> checker, float value, string context, string description)
        {
            if (!(checker.Value <= value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<double> LessThenOrEqual(this ValueChecker<double> checker, double value)
        {
            return LessThenOrEqual(checker, value, null, null);
        }

        public static ValueChecker<double> LessThenOrEqual(this ValueChecker<double> checker, double value, string description)
        {
            return LessThenOrEqual(checker, value, null, description);
        }

        public static ValueChecker<double> LessThenOrEqual(this ValueChecker<double> checker, double value, string context, string description)
        {
            if (!(checker.Value <= value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<decimal> LessThenOrEqual(this ValueChecker<decimal> checker, decimal value)
        {
            return LessThenOrEqual(checker, value, null, null);
        }

        public static ValueChecker<decimal> LessThenOrEqual(this ValueChecker<decimal> checker, decimal value, string description)
        {
            return LessThenOrEqual(checker, value, null, description);
        }

        public static ValueChecker<decimal> LessThenOrEqual(this ValueChecker<decimal> checker, decimal value, string context, string description)
        {
            if (!(checker.Value <= value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<DateTime> LessThenOrEqual(this ValueChecker<DateTime> checker, DateTime value)
        {
            return LessThenOrEqual(checker, value, null, null);
        }

        public static ValueChecker<DateTime> LessThenOrEqual(this ValueChecker<DateTime> checker, DateTime value, string description)
        {
            return LessThenOrEqual(checker, value, null, description);
        }

        public static ValueChecker<DateTime> LessThenOrEqual(this ValueChecker<DateTime> checker, DateTime value, string context, string description)
        {
            if (!(checker.Value <= value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        #endregion

        #region Between

        public static ValueChecker<byte> Between(this ValueChecker<byte> checker, byte firstValue, byte secondValue)
        {
            return Between(checker, firstValue, secondValue, null, null);
        }

        public static ValueChecker<byte> Between(this ValueChecker<byte> checker, byte firstValue, byte secondValue, string description)
        {
            return Between(checker, firstValue, secondValue, null, description);
        }

        public static ValueChecker<byte> Between(this ValueChecker<byte> checker, byte firstValue, byte secondValue, string context, string description)
        {
            if (!(checker.Value >= firstValue && checker.Value <= secondValue))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<int> Between(this ValueChecker<int> checker, int firstValue, int secondValue)
        {
            return Between(checker, firstValue, secondValue, null, null);
        }

        public static ValueChecker<int> Between(this ValueChecker<int> checker, int firstValue, int secondValue, string description)
        {
            return Between(checker, firstValue, secondValue, null, description);
        }

        public static ValueChecker<int> Between(this ValueChecker<int> checker, int firstValue, int secondValue, string context, string description)
        {
            if (!(checker.Value >= firstValue && checker.Value <= secondValue))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<long> Between(this ValueChecker<long> checker, long firstValue, long secondValue)
        {
            return Between(checker, firstValue, secondValue, null, null);
        }

        public static ValueChecker<long> Between(this ValueChecker<long> checker, long firstValue, long secondValue, string description)
        {
            return Between(checker, firstValue, secondValue, null, description);
        }

        public static ValueChecker<long> Between(this ValueChecker<long> checker, long firstValue, long secondValue, string context, string description)
        {
            if (!(checker.Value >= firstValue && checker.Value <= secondValue))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<float> Between(this ValueChecker<float> checker, float firstValue, float secondValue)
        {
            return Between(checker, firstValue, secondValue, null, null);
        }

        public static ValueChecker<float> Between(this ValueChecker<float> checker, float firstValue, float secondValue, string description)
        {
            return Between(checker, firstValue, secondValue, null, description);
        }

        public static ValueChecker<float> Between(this ValueChecker<float> checker, float firstValue, float secondValue, string context, string description)
        {
            if (!(checker.Value >= firstValue && checker.Value <= secondValue))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<double> Between(this ValueChecker<double> checker, double firstValue, double secondValue)
        {
            return Between(checker, firstValue, secondValue, null, null);
        }

        public static ValueChecker<double> Between(this ValueChecker<double> checker, double firstValue, double secondValue, string description)
        {
            return Between(checker, firstValue, secondValue, null, description);
        }

        public static ValueChecker<double> Between(this ValueChecker<double> checker, double firstValue, double secondValue, string context, string description)
        {
            if (!(checker.Value >= firstValue && checker.Value <= secondValue))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<decimal> Between(this ValueChecker<decimal> checker, decimal firstValue, decimal secondValue)
        {
            return Between(checker, firstValue, secondValue, null, null);
        }

        public static ValueChecker<decimal> Between(this ValueChecker<decimal> checker, decimal firstValue, decimal secondValue, string description)
        {
            return Between(checker, firstValue, secondValue, null, description);
        }

        public static ValueChecker<decimal> Between(this ValueChecker<decimal> checker, decimal firstValue, decimal secondValue, string context, string description)
        {
            if (!(checker.Value >= firstValue && checker.Value <= secondValue))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        public static ValueChecker<DateTime> Between(this ValueChecker<DateTime> checker, DateTime firstValue, DateTime secondValue)
        {
            return Between(checker, firstValue, secondValue, null, null);
        }

        public static ValueChecker<DateTime> Between(this ValueChecker<DateTime> checker, DateTime firstValue, DateTime secondValue, string description)
        {
            return Between(checker, firstValue, secondValue, null, description);
        }

        public static ValueChecker<DateTime> Between(this ValueChecker<DateTime> checker, DateTime firstValue, DateTime secondValue, string context, string description)
        {
            if (!(checker.Value >= firstValue && checker.Value <= secondValue))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        #endregion

        #region Into

        public static ValueChecker<T> Into<T>(this ValueChecker<T> checker, T[] arguments)
        {
            return Into<T>(checker, arguments, null, null);
        }

        public static ValueChecker<T> Into<T>(this ValueChecker<T> checker, T[] arguments, string description)
        {
            return Into<T>(checker, arguments, null, description);
        }

        public static ValueChecker<T> Into<T>(this ValueChecker<T> checker, T[] arguments, string context, string description)
        {
            if (arguments.Length == 0 || !arguments.Contains(checker.Value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        #endregion

        #region Expression

        public static ValueChecker<T> Expression<T>(this ValueChecker<T> checker, Func<T, bool> lambda)
        {
            return Expression<T>(checker, lambda, null, null);
        }

        public static ValueChecker<T> Expression<T>(this ValueChecker<T> checker, Func<T, bool> lambda, string description)
        {
            return Expression<T>(checker, lambda, null, description);
        }

        public static ValueChecker<T> Expression<T>(this ValueChecker<T> checker, Func<T, bool> lambda, string context, string description)
        {
            if (!lambda.Invoke(checker.Value))
            {
                checker.AppendError(context, description);
            }
            return checker;
        }

        #endregion

        #region IfSuccess

        public static OperationResult IfSuccess(this OperationResult result, Action operation)
        {
            if (result.Success)
            {
                operation.Invoke();
            }
            return result;
        }

        public static OperationResult IfSuccess<T>(this ValueChecker<T> checker, Action operation)
        {
            if (checker.Result.Success)
            {
                operation.Invoke();
            }
            return checker.Result;
        }

        public static OperationResult<T> IfSuccess<T>(this OperationResult<T> result, Action<T> operation)
        {
            if (result.Success)
            {
                operation(result.Value);
            }
            return result;
        }

        public static OperationResult<T> IfSuccess<T>(this OperationResult result, Func<OperationResult<T>> operation)
        {
            return result.Success ? operation.Invoke() : (OperationResult<T>)result;
        }

        public static OperationResult<T> IfSuccess<T,K>(this ValueChecker<K> checker, Func<OperationResult<T>> operation)
        {
            return checker.Result.Success ? operation.Invoke() : (OperationResult<T>)checker.Result;
        }

        #endregion

        #region IfFailed

        public static OperationResult IfFailed(this OperationResult result, Action<OperationResult> operation)
        {
            if (!result.Success)
            {
                operation(result);
            }
            return result;
        }

        public static OperationResult IfFailed<T>(this ValueChecker<T> checker, Action<OperationResult> operation)
        {
            if (!checker.Result.Success)
            {
                operation(checker.Result);
            }
            return checker.Result;
        }

        public static OperationResult<T> IfFailed<T>(this OperationResult<T> result, Action<OperationResult<T>> operation)
        {
            if (!result.Success)
            {
                operation(result);
            }
            return result;
        }

        #endregion
    }
}
