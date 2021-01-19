using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Eml.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Case insensitive comparison. Trims both words before comparing.
        /// </summary>
        public static bool IsEqualTo(this string source, string value)
        {
            return string.Equals(source.Trim(), value.Trim(), StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Shorthand for string.IsNullOrWhiteSpace
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// Removes the trimValue before comparing. Case insensitive comparison.
        /// </summary>
        public static bool IsEqualTo(this string source, string value, string trimValue)
        {
            return source.Replace(trimValue, string.Empty).IsEqualTo(value.Replace(trimValue, string.Empty));
        }

        /// <summary>
        /// Case insensitive comparison.
        /// </summary>
        public static bool IsWithStart(this string source, string value)
        {
            return source.StartsWith(value, true, null);
        }

        /// <summary>
        /// Case insensitive comparison.
        /// </summary>
        public static bool IsContains(this string source, string value)
        {
            if (value.IsNullOrWhiteSpace())
            {
                return source.IsNullOrWhiteSpace();
            }

            return source.Contains(value, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Find and replace using regexPattern.
        /// </summary>
        public static string FindReplaceUsingRegEx(this string body, string regexPattern, string value)
        {
            body = Regex.Replace(body, regexPattern, value);

            return body;
        }

        /// <summary>
        /// Creates a KeyValuePair.
        /// </summary>
        public static KeyValuePair<string, T> KeyValuePair<T>(this string key, T value)
        {
            return new KeyValuePair<string, T>(key, value);
        }

        /// <summary>
        ///     Lowercase the first letter.
        /// </summary>
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
        public static string UppercaseFirst(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;

            var a = s.ToCharArray();

            a[0] = char.ToUpper(a[0]);

            return new string(a);
        }

        /// <summary>
        /// Capitalize all words using '.' as the default word boundaries.
        /// </summary>
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
            var lastLetter = a[^1];
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
        /// Remove string at the end. Case insensitive.
        /// </summary>
        public static string TrimRight(this string text, string tail)
        {
            var index = text.IndexOf(tail, StringComparison.CurrentCultureIgnoreCase);

            if (index > 0) text = text.Substring(0, index);

            return text;
        }

        /// <summary>
        /// Remove string at the start. Case insensitive.
        /// </summary>
        public static string TrimLeft(this string text, string head)
        {
            var cnt = head.Length;
            var tmpText = text.Substring(0, cnt);

            return tmpText.IsEqualTo(head) ? text.Substring(cnt) : text;
        }

        /// <summary>
        /// Used to create jwt. Encode UTF8.
        /// </summary>
        /// <param name="textToEncode"></param>
        /// <returns></returns>
        public static string Base64Encode(this string textToEncode)
        {
            var textAsBytes = Encoding.UTF8.GetBytes(textToEncode);

            return Convert.ToBase64String(textAsBytes);
        }

        /// <summary>
        ///  Decode UTF8.
        /// </summary>
        /// <param name="encodedText"></param>
        /// <returns></returns>
        public static string Base64Decode(this string encodedText)
        {
            var stringAsBytes = Convert.FromBase64String(encodedText);
            var returnValue = Encoding.UTF8.GetString(stringAsBytes);

            return returnValue;
        }

        /// <summary>
        /// Used by http helpers. Create Basic authorization token. Adds 'Basic ' prefix.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string ToBasicAuthenticationEncode(this string userName, string password)
        {
            var token = userName.ToBase64AuthenticationToken(password);
            var basicAuthentication = $"Basic {token}";

            return basicAuthentication;
        }

        /// <summary>
        /// Used by http helpers. Create Base64 authorization token using {userName}:{password}. 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string ToBase64AuthenticationToken(this string userName, string password)
        {
            var token = $"{userName}:{password}".Base64Encode();

            return token;
        }

        public static string ToStringWithHeaderMessage(this IEnumerable<string> items, string msg)
        {
            var enumerable = items.ToArray();
            var rowCount = enumerable.Length;

            return $"{Environment.NewLine}({rowCount}){msg}: {Environment.NewLine}    {string.Join(Environment.NewLine + "    ", enumerable.ToArray())}{Environment.NewLine}";
        }

        /// <summary>
        /// Serialize objects with preferred options:
        /// <para>NullValueHandling.Ignore</para>
        /// <para>ReferenceLoopHandling.Ignore</para>
        /// <para>new CamelCasePropertyNamesContractResolver()</para>
        /// </summary>
        public static string Serialize<T>(this T obj)
        {
            var options = new JsonSerializerSettings();

            options.SetDefaultOptions();

            var objAsString = JsonConvert.SerializeObject(obj, options);

            return objAsString;
        }

        /// <summary>
        /// Apply preferred options:
        /// <para>NullValueHandling.Ignore</para>
        /// <para>ReferenceLoopHandling.Ignore</para>
        /// <para>new CamelCasePropertyNamesContractResolver()</para>
        /// </summary>
        public static void SetDefaultOptions(this JsonSerializerSettings options)
        {
            options.NullValueHandling = NullValueHandling.Ignore;
            options.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public static string GetSortColumnErrorMessage(this string sortColumn, string enumName)
        {
            return $"sortColumn: [{sortColumn}] is not supported. Add this to {enumName}.";
        }

        /// <summary>
        /// Combine list of strings using new line. Pass-in tabs or spaces for the prefix.
        /// </summary>
        public static string Join(this IEnumerable<string> strings, string prefix = "")
        {
            var list = strings.ToList();

            if (!string.IsNullOrWhiteSpace(prefix))
            {
                list = list.ConvertAll(r => $"{prefix}{r}");
            }

            return string.Join($"{Environment.NewLine}", list.ToArray());
        }
    }
}