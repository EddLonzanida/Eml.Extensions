﻿using System;
using System.Globalization;

namespace Eml.Extensions
{
    public static class DateExtensions
    {
        private const string STANDARD_DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

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
        public static DateTime ToBeforeMidnight(this DateTime source)
        {
            var d = source.ToString("yyyy-MM-dd 23:59:59");

            return DateTime.ParseExact(d, STANDARD_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
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
        public static DateTime? ToBeforeMidnight(this DateTime? source)
        {
            if (!source.HasValue)
            {
                return null;
            }

            var d = source?.ToString("yyyy-MM-dd 23:59:59");

            return DateTime.ParseExact(d, STANDARD_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// DateTime.TryParse(dateAsString, out var date) 
        /// </summary>
        public static DateTime? ToDateTime(this string dateAsString)
        {
            if (DateTime.TryParse(dateAsString, out var date))
            {
                return date;
            }

            return null;
        }

        public static DateTime? ToDateTime(this string dateAsString, string format)
        {
            if (dateAsString.IsNullOrWhiteSpace())
            {
                return null;
            }

            if (DateTime.TryParse(dateAsString, out var date))
            {
                return date;
            }

            return DateTime.ParseExact(dateAsString ?? string.Empty, format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Compare <paramref name="date1"/> with <paramref name="date2"/>.
        /// <inheritdoc cref="ToComparableDateTime"/>
        /// </summary>
        public static bool IsEqualTo(this DateTime date1, DateTime date2, int decimalPlaces = 2)
        {
            var formattedDate1 = date1.ToComparableDateTime(decimalPlaces);
            var formattedDate2 = date2.ToComparableDateTime(decimalPlaces);
            var isEqual = formattedDate1 == formattedDate2;

            return isEqual;
        }

        /// <summary>
        /// <para>Round off the milliseconds before comparing.</para>
        /// <para>This is a workaround because the 'fff' date format simply truncates the milliseconds and does not perform rounding off.</para>
        /// </summary>
        public static string ToComparableDateTime(this DateTime date1, int decimalPlaces = 2)
        {
            var ms = new string('F', decimalPlaces + 1);
            var ms1 = decimal.Parse($"0.{date1.ToString(ms)}") // truncate milliseconds to decimalPlaces + 1
                .ToString($"F{decimalPlaces}")  // round off
                .Replace("0.", string.Empty);   // remove decimal points
            var dt1 = date1.ToString(STANDARD_DATE_TIME_FORMAT);
            var formattedDate1 = $"{dt1}.{ms1}";

            return formattedDate1;
        }
    }
}