using System;
using System.Globalization;

namespace Eml.Extensions
{
    public static class DateExtensions
    {
        private static string ToStringOrDefault(this DateTime? source, string format, string defaultValue)
        {
            if (source != null)
            {
                return source.Value.ToString(format);
            }

            return string.IsNullOrEmpty(defaultValue) ? string.Empty : defaultValue;
        }

        public static string ToStringOrDefault(this DateTime? source, string format)
        {
            return ToStringOrDefault(source, format, null);
        }

        /// <summary>
        /// Returns 23:59:59 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTime ToBeforeMidnight(this DateTime source)
        {
            var d = source.ToString("yyyy-MM-dd 23:59:59");

            return DateTime.ParseExact(d, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns 23:59:59 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTime? ToBeforeMidnight(this DateTime? source)
        {
            if (!source.HasValue)
            {
                return null;
            }

            var d = source?.ToString("yyyy-MM-dd 23:59:59");

            return DateTime.ParseExact(d, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}