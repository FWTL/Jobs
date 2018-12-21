using System;
using System.Globalization;

namespace FWTL.Core.Extensions
{
    public static class StringExtensions
    {
        public static T To<T>(this string source)
        {
            return (T)Convert.ChangeType(source, typeof(T), CultureInfo.InvariantCulture);
        }

        public static T? ToEnum<T>(this string source) where T : struct
        {
            T value;
            if (Enum.TryParse(source.ToUpper(), out value))
            {
                return value;
            }

            return null;
        }

        public static T? ToN<T>(this string source) where T : struct
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                try
                {
                    return (T)Convert.ChangeType(source, typeof(T), CultureInfo.InvariantCulture);
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }
    }
}
