using System;

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
    }
}