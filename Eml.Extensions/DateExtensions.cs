using System;
using System.Globalization;

namespace Eml.Extensions
{
    public static class DateExtensions
    {
        /// <summary>
        /// Format date using the specified format. Returns empty string if date is null.
        /// </summary>
        private static string ToStringOrDefault(this DateTime? source, string format, string defaultValue)
        {
            if (source != null)
            {
                return source.Value.ToString(format);
            }

            return string.IsNullOrEmpty(defaultValue) ? string.Empty : defaultValue;
        }

        /// <summary>
        /// <inheritdoc cref="ToStringOrDefault(DateTime? , string , string)"/>
        /// </summary>
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
        /// Returns yyyy-MM-dd 11:59:59 PM
        /// </summary>
        public static string ToStringBeforeMidnightPm(this DateTime source)
        {
            var d = source.ToString("yyyy-MM-dd 11:59:59", CultureInfo.InvariantCulture);
            var beforeMidNight = $"{d} PM";

            return beforeMidNight;
        }

        /// <summary>
        /// Returns military time yyyy-MM-dd 23:59:59
        /// </summary>
        public static string ToStringBeforeMidnight(this DateTime source)
        {
            var beforeMidNight = source.ToString("yyyy-MM-dd 23:59:59", CultureInfo.InvariantCulture);

            return beforeMidNight;
        }

        /// <summary>
        /// Returns 23:59:59 
        /// </summary>
        /// <param name="source"></param>
        public static DateTime? ToBeforeMidnight(this DateTime? source)
        {
            if (!source.HasValue)
            {
                return null;
            }

            var d = source?.ToString("yyyy-MM-dd 23:59:59");

            return DateTime.ParseExact(d, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public static DateTime? ToDateTime(this string dateAsString)
        {
            if (DateTime.TryParse(dateAsString, out var date))
            {
                return date;
            }

            return null;
        }
    }
}