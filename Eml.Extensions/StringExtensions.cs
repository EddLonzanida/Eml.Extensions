using System;
using System.Collections.Generic;
using System.Linq;
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

        public static string ToProperCase(this string s, char delimiter = '.')
        {
            var result = s.ToLower();
            var aWords = new List<string>();

            aWords.AddRange(result.Split(delimiter));

            if (aWords.Count < 1) return s;
            if (aWords.Count == 1) return result.UppercaseFirst();

            aWords = aWords.ConvertAll(UppercaseFirst);
            result = string.Join(delimiter.ToString(), aWords.ToArray());

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

        public static string Pluralize(this string text)
        {
            var newWord = text.Trim();
            var word = text.Trim();
            var wordLowered = word.ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(word)) return string.Empty;

            var a = word.ToCharArray();
            var lastLetter = a[a.Length - 1];
            var isLower = char.IsLower(lastLetter);
            var append = "";

            var exceptions = new Dictionary<string, string> {
                { "man", "men" },
                { "woman", "women" },
                { "child", "children" },
                { "tooth", "teeth" },
                { "foot", "feet" },
                { "mouse", "mice" },
                { "belief", "beliefs" } };

            if (exceptions.ContainsKey(wordLowered))
            {
                return exceptions[wordLowered];
            }

            // If it's already pluralized
            if (exceptions.Where(w => w.Value == wordLowered).ToList().Count > 0)
            {
                return text;
            }

            if (word.EndsWith("s", StringComparison.OrdinalIgnoreCase))
            {
                return text;
            }

            if (word.EndsWith("y", StringComparison.OrdinalIgnoreCase) &&
                !word.EndsWith("ay", StringComparison.OrdinalIgnoreCase) &&
                !word.EndsWith("ey", StringComparison.OrdinalIgnoreCase) &&
                !word.EndsWith("iy", StringComparison.OrdinalIgnoreCase) &&
                !word.EndsWith("oy", StringComparison.OrdinalIgnoreCase) &&
                !word.EndsWith("uy", StringComparison.OrdinalIgnoreCase))
            {
                append = isLower ? "ies" : "IES";

                return newWord.Substring(0, newWord.Length - 1) + append;
            }

            if (word.EndsWith("us", StringComparison.InvariantCultureIgnoreCase)
                || word.EndsWith("ss", StringComparison.InvariantCultureIgnoreCase)
                || word.EndsWith("x", StringComparison.InvariantCultureIgnoreCase)
                || word.EndsWith("ch", StringComparison.InvariantCultureIgnoreCase)
                || word.EndsWith("sh", StringComparison.InvariantCultureIgnoreCase))
            {
                append = isLower ? "es" : "ES";

                return newWord + append;
            }

            if (word.EndsWith("s", StringComparison.InvariantCultureIgnoreCase))
            {
                return newWord;
            }

            if (word.EndsWith("f", StringComparison.InvariantCultureIgnoreCase) && word.Length > 1)
            {
                append = isLower ? "ves" : "VES";

                return newWord.Substring(0, newWord.Length - 1) + append;
            }

            if (word.EndsWith("fe", StringComparison.InvariantCultureIgnoreCase) && word.Length > 2)
            {
                append = isLower ? "ves" : "VES";

                return newWord.Substring(0, newWord.Length - 2) + append;
            }

            append = isLower ? "s" : "S";

            return newWord + append;
        }

        /// <summary>
        /// Get the last n characters of a string.
        /// </summary>
        public static string GetLast(this string text, int charCount)
        {
            return charCount >= text.Length ? text : text.Substring(text.Length - charCount);
        }

        /// <summary>
        /// Remove string at the end.
        /// </summary>
        public static string TrimRight(this string text, string tail)
        {
            var index = text.IndexOf(tail, StringComparison.CurrentCultureIgnoreCase);

            if (index > 0) text = text.Substring(0, index);

            return text;
        }
    }
}