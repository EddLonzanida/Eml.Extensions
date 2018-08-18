using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#if NETCORE
using System.Reflection;
#endif

namespace Eml.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Case insensitive comparison.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEqualTo(this string source, string value)
        {
            return string.Equals(source.ToLower(), value.ToLower(), StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Removes the trimValue before comparing. Case insensitive comparison.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <param name="trimValue"></param>
        /// <returns></returns>
        public static bool IsEqualTo(this string source, string value, string trimValue)
        {
            return source.Replace(trimValue, string.Empty).IsEqualTo(value.Replace(trimValue, string.Empty));
        }

        /// <summary>
        /// Creates a KeyValuePair.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static KeyValuePair<string, T> KeyValuePair<T>(this string key, T value)
        {
            return new KeyValuePair<string, T>(key, value);
        }

        /// <summary>
        ///     Lowercase the first letter.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string LowercaseFirst(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;

            var a = s.ToCharArray();

            a[0] = char.ToLower(a[0]);

            return new string(a);
        }

        /// <summary>
        ///     Uppercase the first letter.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UppercaseFirst(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;

            var a = s.ToCharArray();

            a[0] = char.ToUpper(a[0]);

            return new string(a);
        }

        public static string ToProperCase(this string s, char delimeter = '.')
        {
            var result = s;
            var aWords = new List<string>();

            aWords.AddRange(s.Split(delimeter));

            if (aWords.Count < 1) return result;
            if (aWords.Count == 1) return s.UppercaseFirst();

            aWords = aWords.ConvertAll(UppercaseFirst);
            result = string.Join(delimeter.ToString(), aWords.ToArray());

            return result;
        }

        /// <summary>
        /// Uses capital letter as an indicator.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string ToSpaceDelimitedWords(this string word)
        {
            var result = Regex.Replace(word, @"\s\s+", " ");

            result = Regex.Replace(result, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ")
               .ToProperCase(' ')
               .Trim();

            return result;
        }

        public static void TrimStringProperties<T>(this T objInstance)
            where T : class
        {
            var objType = objInstance.GetType();
            var props = objType.GetProperties();

            foreach (var p in props)
            {
                var propertyType = p.PropertyType.Name;

                if (propertyType != "String" || !p.CanWrite) continue;

                var oValue = p.GetValue(objInstance);

                if (oValue == null) continue;

                var propertyValue = oValue.ToString().Trim();

                p.SetValue(objInstance, propertyValue);
            }
        }
    }
}