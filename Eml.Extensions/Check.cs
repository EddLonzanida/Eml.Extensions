using System.Runtime.CompilerServices;

namespace Eml.Extensions;

public static class Check
{
    /// <summary>
    ///     Throws an ArgumentNullException if <paramref name="value" /> is null.
    /// </summary>
    public static T CheckNotNull<T>(this T value, [CallerArgumentExpression(nameof(value))] string parameterName = "")
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName);
        }

        return value;
    }

    /// <summary>
    ///     Throws an ArgumentNullException if <paramref name="value" /> is null, empty or whitespace.
    /// </summary>
    public static string CheckNotEmpty(this string? value, [CallerArgumentExpression(nameof(value))] string parameterName = "")
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"The argument '{parameterName}' cannot be null, empty or contain only white space.");
        }

        return value;
    }
}
