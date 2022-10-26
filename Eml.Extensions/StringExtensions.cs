using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace Eml.Extensions;

public static class StringExtensions
{
    /// <summary>
    ///     Case insensitive comparison. Trims both words before comparing.
    /// </summary>
    public static bool IsEqualTo(this string? source, string? value)
    {
        return string.Equals(source?.Trim(), value?.Trim(), StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>
    ///     Shorthand for string.IsNullOrWhiteSpace
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string source)
    {
        return string.IsNullOrWhiteSpace(source);
    }

    /// <summary>
    ///     Removes the trimValue before comparing. Case insensitive comparison.
    /// </summary>
    public static bool IsEqualTo(this string source, string value, string trimValue)
    {
        return source?.Replace(trimValue, string.Empty)
            .IsEqualTo(value?.Replace(trimValue, string.Empty)) ?? false;
    }

    /// <summary>
    ///     Case insensitive comparison.
    /// </summary>
    public static bool IsWithStart(this string source, string value)
    {
        return source?.StartsWith(value, true, null) ?? false;
    }

    /// <summary>
    ///     Case insensitive comparison.
    /// </summary>
    public static bool IsContains(this string? source, string value)
    {
        if (value.IsNullOrWhiteSpace())
        {
            return string.IsNullOrWhiteSpace(source);
        }

        if (!string.IsNullOrWhiteSpace(source))
        {
            return source.Contains(value, StringComparison.CurrentCultureIgnoreCase);
        }

        return false;
    }

    /// <summary>
    ///     Find and replace using regexPattern.
    /// </summary>
    public static string FindReplaceUsingRegEx(this string body, string regexPattern, string value)
    {
        var tmpBody = Regex.Replace(body, regexPattern, value);

        return tmpBody;
    }

    public static string FindReplaceUsingRegEx(this string body, List<string> regexPatterns, string value)
    {
        if (!regexPatterns.Any())
        {
            return body;
        }

        foreach (var regEx in regexPatterns)
        {
            body = body.FindReplaceUsingRegEx(regEx, value);
        }

        return body;
    }

    public static string FindReplaceUsingRegEx(this string body, List<KeyValuePair<string, string>> regexPatterns)
    {
        if (!regexPatterns.Any())
        {
            return body;
        }

        foreach (var regEx in regexPatterns)
        {
            body = body.FindReplaceUsingRegEx(regEx.Key, regEx.Value);
        }

        return body;
    }

    /// <summary>
    ///     Creates a KeyValuePair.
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
        if (string.IsNullOrWhiteSpace(s))
        {
            return string.Empty;
        }

        var a = s.ToCharArray();

        a[0] = char.ToLower(a[0]);

        return new string(a);
    }

    /// <summary>
    ///     Uppercase the first letter.
    /// </summary>
    public static string UppercaseFirst(this string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return string.Empty;
        }

        var a = s.ToCharArray();

        a[0] = char.ToUpper(a[0]);

        return new string(a);
    }

    /// <summary>
    ///     Capitalize all words using '.' as the default word boundaries.
    /// </summary>
    public static string ToProperCase(this string s, char delimiter = '.')
    {
        var result = s.ToLower();
        var aWords = new List<string>();

        aWords.AddRange(result.Split(delimiter));

        switch (aWords.Count)
        {
            case < 1:
                return s;

            case 1:
                return result.UppercaseFirst();
        }

        aWords = aWords.ConvertAll(UppercaseFirst);
        result = string.Join(delimiter.ToString(), aWords.ToArray());

        return result;
    }

    /// <summary>
    ///     Uses capital letter as an indicator.
    /// </summary>
    public static string ToSpaceDelimitedWords(this string word)
    {
        const string delimiter = " ";

        var result = Regex.Replace(word, @"\s\s+", " ");

        result = Regex.Replace(result, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ")
            .Trim();

        var aWords = new List<string>();

        aWords.AddRange(result.Split(delimiter));

        switch (aWords.Count)
        {
            case < 1:
                return result;

            case 1:
                return result.UppercaseFirst();
        }

        aWords = aWords.Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Select(UppercaseFirst).ToList();

        result = aWords.ToDelimitedString(delimiter);

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

            if (propertyType != "String" || !p.CanWrite)
            {
                continue;
            }

            var oValue = p.GetValue(objInstance);

            if (oValue == null)
            {
                continue;
            }

            var propertyValue = oValue.ToString()?.Trim();

            p.SetValue(objInstance, propertyValue);
        }
    }

    public static string Pluralize(this string text)
    {
        var newWord = text.Trim();
        var word = text.Trim();
        var wordLowered = word.ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(word))
        {
            return string.Empty;
        }

        var a = word.ToCharArray();
        var lastLetter = a[^1];
        var isLower = char.IsLower(lastLetter);
        string append;

        var exceptions = new Dictionary<string, string>
        {
            { "man", "men" },
            { "woman", "women" },
            { "child", "children" },
            { "tooth", "teeth" },
            { "foot", "feet" },
            { "mouse", "mice" },
            { "belief", "beliefs" }
        };

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
    ///     Get the last <paramref name="charCount" /> characters of a string.
    /// </summary>
    public static string Right(this string text, int charCount)
    {
        return charCount >= text.Length ? text : text[^charCount..];
    }

    public static string? Right(this string? text, string rightText)
    {
        var charCount = rightText.Length;

        if (string.IsNullOrWhiteSpace(text))
        {
            return text;
        }

        return text.Right(charCount);
    }

    public static string? Left(this string? text, string leftText)
    {
        var charCount = leftText.Length;

        if (string.IsNullOrWhiteSpace(text))
        {
            return text;
        }

        return charCount >= text.Length ? text : text[..charCount];
    }

    /// <summary>
    ///     Remove string at the end. Case insensitive.
    /// </summary>
    public static string TrimRight(this string text, string tail)
    {
        var index = text.IndexOf(tail, StringComparison.CurrentCultureIgnoreCase);

        if (index > 0)
        {
            text = text[..index];
        }

        return text;
    }

    /// <summary>
    ///     Remove string at the start. Case insensitive.
    /// </summary>
    public static string TrimLeft(this string text, string? head)
    {
        var cnt = head?.Length ?? 0;

        if (cnt > text.Length || cnt == 0)
        {
            return string.Empty;
        }

        var tmpText = text[..cnt];

        return tmpText.IsEqualTo(head) ? text[cnt..] : text;
    }

    /// <summary>
    ///     Used to create jwt. Encode UTF8.GetBytes.
    /// </summary>
    public static string Base64Encode(this string textToEncode)
    {
        var textAsBytes = Encoding.UTF8.GetBytes(textToEncode);

        return Convert.ToBase64String(textAsBytes);
    }

    /// <summary>
    ///     Decode using UTF8.GetString.
    ///     <inheritdoc cref="Convert.FromBase64String" />
    /// </summary>
    public static string Base64Decode(this string encodedText)
    {
        var stringAsBytes = Convert.FromBase64String(encodedText);
        var returnValue = Encoding.UTF8.GetString(stringAsBytes);

        return returnValue;
    }

    /// <summary>
    ///     <para>Create Basic authorization token. Adds 'Basic ' prefix.</para>
    ///     <inheritdoc cref="ToBase64AuthenticationToken" />
    /// </summary>
    public static string ToBasicAuthenticationEncode(this string userName, string password)
    {
        var token = userName.ToBase64AuthenticationToken(password);
        var basicAuthentication = $"Basic {token}";

        return basicAuthentication;
    }

    /// <summary>
    ///     <para>Create Base64 authorization token using {userName}:{password}.</para>
    ///     <inheritdoc cref="Base64Encode" />
    /// </summary>
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
    ///     Join <paramref name="items" /> separated by a new line.
    ///     <para>Pass-in comma or pipe for the delimiter.</para>
    /// </summary>
    public static string ToDelimitedString<T>(this IEnumerable<T> items, string delimiter = "")
    {
        var enumerable = items.ToList()
            .ConvertAll(x => x?.ToString() ?? string.Empty)
            .ToArray();

        var defaultDelimiter = string.Empty;

        delimiter = delimiter == defaultDelimiter ? Environment.NewLine : delimiter;

        return string.Join(delimiter, enumerable.ToArray());
    }

    /// <summary>
    ///     Serialize <typeparamref name="T" /> with the following options:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>
    ///                 <see cref="NullValueHandling.Ignore" />
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 <see cref="ReferenceLoopHandling.Ignore" />
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 <see cref="Formatting.Indented" />
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 <code>new <see cref="CamelCasePropertyNamesContractResolver" />()</code>
    ///             </description>
    ///         </item>
    ///     </list>
    /// </summary>
    public static string Serialize<T>(this T obj, bool showNullValues = false, bool indented = false, bool usePascalCase = false)
    {
        var options = new JsonSerializerSettings();

        options.SetDefaultOptions(showNullValues, indented, usePascalCase);

        var objAsString = obj == null ? string.Empty : JsonConvert.SerializeObject(obj, options);

        return objAsString;
    }

    /// <summary>
    ///     Set default options to:
    ///     <para>
    ///         <see cref="NullValueHandling.Ignore" />
    ///     </para>
    ///     <para>
    ///         <see cref="ReferenceLoopHandling.Ignore" />
    ///     </para>
    ///     <para>
    ///         <see cref="Formatting.Indented" />
    ///     </para>
    ///     <code>if (!usePascalCase) new <see cref="CamelCasePropertyNamesContractResolver" />()</code>
    /// </summary>
    public static void SetDefaultOptions(this JsonSerializerSettings options, bool showNullValues = false, bool indented = true, bool usePascalCase = false)
    {
        if (!showNullValues)
        {
            options.NullValueHandling = NullValueHandling.Ignore;
        }

        if (indented)
        {
            options.Formatting = Formatting.Indented;
        }

        options.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        if (!usePascalCase)
        {
            options.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }

    public static string GetSortColumnErrorMessage(this string sortColumn, string enumName)
    {
        return $"sortColumn: [{sortColumn}] is not supported. Add this to {enumName}.";
    }

    /// <summary>
    ///     Combine list of strings using new line. Pass-in tabs or spaces for the prefix.
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

    /// <summary>
    /// Calls Convert.ToHexString(byteArray) and prefix with 0x.
    /// </summary>
    public static string GetString(this byte[]? byteArray, bool ignoreNull)
    {
        if (byteArray == null && !ignoreNull)
        {
            throw new Exception($"The parameter {nameof(byteArray)} is null.");
        }

        if (byteArray == null)
        {
            return string.Empty;
        }

        var byteAsString = Convert.ToHexString(byteArray);

        return $"0x{byteAsString}";
    }

    /// <summary>
    /// <inheritdoc cref="GetString(byte[],bool)"/>
    /// </summary>
    public static string GetString(this byte[]? byteArray)
    {
        return byteArray.GetString(false);
    }

    /// <summary>
    ///     <para>Used to get the callsite for Logging purposes.</para>
    ///     <para>Will search for files within the specified <paramref name="applicationRootNamespace" />.</para>
    ///     <para>Will check InnerException first.</para>
    ///     <para>Example:</para>
    ///     <code language="c#">
    ///      var callSite = e.GetCallSite(AppConstants.ApplicationRootNamespace, new[] { "NuGets", "Helpers", "Extensions" });
    ///     </code>
    /// </summary>
    public static string GetCallSite(this Exception exception, string applicationRootNamespace, string[]? excludedPaths = null)
    {
        var regExPattern = $@".*[\\\/]{applicationRootNamespace}\..*";

        if (excludedPaths is { Length: > 0 })
        {
            var excludedPathsAsString = excludedPaths.ToDelimitedString("|");

            regExPattern = $"(?!.*({excludedPathsAsString}).*){regExPattern}";
        }

        var stackTraceSplit = (exception.InnerException?.StackTrace ?? exception.StackTrace)?.Split(Environment.NewLine);
        var callSite = stackTraceSplit?.FirstOrDefault(x => Regex.IsMatch(x, regExPattern, RegexOptions.IgnoreCase))?.Trim() ?? string.Empty;
        var aCallSite = callSite.Split(" in ");

        callSite = aCallSite.LastOrDefault()?.Trim() ?? string.Empty;

        return callSite;
    }

    /// <summary>
    ///     <para>Get the Exception Message.</para>
    ///     <para>Will check InnerException first.</para>
    /// </summary>
    public static string GetErrorMessage(this Exception exception)
    {
        var errorMessage = exception.InnerException?.Message ?? exception.Message;

        return errorMessage;
    }
}
