using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SharedRef.Helper
{
    /// <summary>
    /// Проверки с выбросом исключений (аналог NUnit, но с русским переводом и общим типом исключения)
    /// </summary>
    public static class Assert
    {
        public static void IsNotEqual(object o1, object o2, string message = "Ошибка", params object[] args)
        {
            if (o1 == o2)
                throw new ApplicationException(string.Format(message, args));
        }

        public static void IsEqual(object o1, object o2, string message = "Ошибка", params object[] args)
        {
            if (o1 != o2)
                throw new ApplicationException(string.Format(message, args));
        }

        public static void IsNotNull(object o1, string message = "Ошибка", params object[] args)
        {
            IsNotEqual(o1, null, message, args);
        }

        public static void IsNull(object o1, string message = "Ошибка", params object[] args)
        {
            IsEqual(o1, null, message, args);
        }

        public static void IsNotNullOrEmpty(string o1, string message = "Ошибка", params object[] args)
        {
            if (string.IsNullOrEmpty(o1))
                throw new ApplicationException(string.Format(message = "Ошибка", args));
        }

        public static void True(bool p, string message = "Ошибка", params object[] args)
        {
            if (p == false)
                throw new ApplicationException(string.Format(message, args));
        }

        public static void False(bool p, string message = "Ошибка", params object[] args)
        {
            if (p)
                throw new ApplicationException(string.Format(message, args));
        }

        public static void Less(IComparable o1, IComparable o2, string message = "Ошибка", params object[] args)
        {
            if (o1.CompareTo(o2) >= 0)
                throw new ApplicationException(string.Format(message, args));
        }

        public static void Greater(IComparable o1, IComparable o2, string message = "Ошибка", params object[] args)
        {
            if (o1.CompareTo(o2) <= 0)
                throw new ApplicationException(string.Format(message, args));
        }

        public static void IsInstanceOf<T>(object o, string message = "Ошибка", params object[] args)
        {
            if (o is T == false)
                throw new ApplicationException(string.Format(message, args));
        }

        public static void IsNotInstanceOf<T>(object o, string message = "Ошибка", params object[] args)
        {
            if (o is T)
                throw new ApplicationException(string.Format(message, args));
        }

        public static void Fail(string message = "Ошибка", params object[] args)
        {
            throw new ApplicationException(string.Format(message, args));
        }

        public static void Zero(int o1, string message = "Ошибка", params object[] args)
        {
            if (o1 != 0)
                throw new ApplicationException(string.Format(message, args));
        }

        public static void NotZero(int o1, string message = "Ошибка", params object[] args)
        {
            if (o1 == 0)
                throw new ApplicationException(string.Format(message, args));
        }

        public static void NotEmpty(ICollection o1, string message = "Ошибка", params object[] args)
        {
            if (o1.Count == 0)
                throw new ApplicationException(string.Format(message, args));
        }

        public static void Empty(ICollection o1, string message = "Ошибка", params object[] args)
        {
            if (o1.Count != 0)
                throw new ApplicationException(string.Format(message, args));
        }
    }
}

