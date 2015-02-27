using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedRef.Extensions.ObjectHelper
{
    /// <summary>
    /// Расширения для более простого обхода дерева объектов
    /// </summary>
    public static class ObjectHelperExtension
    {
        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o);
        }

        public static TResult Return<TInput, TResult>(this TInput o,
            Func<TInput, TResult> evaluator, TResult failureValue) where TInput : class
        {
            if (o == null) return failureValue;
            return evaluator(o);
        }

        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? o : null;
        }

        public static TInput Unless<TInput>(this TInput o, Func<TInput, bool> evaluator)
          where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? null : o;
        }

        public static TInput Do<TInput>(this TInput o, Action<TInput> action)
            where TInput : class
        {
            if (o == null) return null;
            action(o);
            return o;
        }

        public static TResult TightCast<TResult>(this object o)
        {
            return (TResult)o;
        }

        public static TResult WeakCast<TResult>(this object o)
            where TResult : class
        {
            if (o == null) return null;
            return (o as TResult);
        }
    }
}
