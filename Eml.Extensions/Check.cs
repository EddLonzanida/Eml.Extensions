using System;

namespace Eml.Extensions
{
    public static class Check
    {
        /// <summary>
        /// Throws an ArgumentNullException if value is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static T CheckNotNull<T>(this T value, string parameterName)
        {
            if (value == null) throw new ArgumentNullException(parameterName);

            return value;
        }

        /// <summary>
        /// Throws an ArgumentNullException if string is null.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static string CheckNotEmpty(this string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"The argument '{parameterName}' cannot be null, empty or contain only white space.");
            }

            return value;
        }
    }
}