using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Eml.Extensions;

public static class DateExtensions
{
    public const string STANDARD_DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

    public const string STANDARD_DATE = "yyyy-MM-dd";

    /// <summary>
    ///     Format date using the specified format. Returns empty string if date is null.
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
    ///     <inheritdoc cref="ToStringOrDefault(DateTime? , string , string)" />
    /// </summary>
    public static string ToStringOrDefault(this DateTime? source, string format)
    {
        return ToStringOrDefault(source, format, null);
    }

    /// <summary>
    ///     Returns 23:59:59
    /// </summary>
    public static DateTime ToBeforeMidnight(this DateTime source)
    {
        var d = source.ToString("yyyy-MM-dd 23:59:59");

        return DateTime.ParseExact(d, STANDARD_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Returns yyyy-MM-dd 11:59:59 PM
    /// </summary>
    public static string ToStringBeforeMidnightPm(this DateTime source)
    {
        var d = source.ToString("yyyy-MM-dd 11:59:59", CultureInfo.InvariantCulture);
        var beforeMidNight = $"{d} PM";

        return beforeMidNight;
    }

    /// <summary>
    ///     Returns military time yyyy-MM-dd 23:59:59
    /// </summary>
    public static string ToStringBeforeMidnight(this DateTime source)
    {
        var beforeMidNight = source.ToString("yyyy-MM-dd 23:59:59", CultureInfo.InvariantCulture);

        return beforeMidNight;
    }

    /// <summary>
    ///     Returns military time yyyy-MM-dd 23:59:59
    /// </summary>
    public static string ToStringBeforeMidnight(this DateTime? source)
    {
        var beforeMidNight = source?.ToString("yyyy-MM-dd 23:59:59", CultureInfo.InvariantCulture);

        return beforeMidNight;
    }

    /// <summary>
    ///     Returns 23:59:59
    /// </summary>
    public static DateTime? ToBeforeMidnight(this DateTime? source)
    {
        if (!source.HasValue)
        {
            return null;
        }

        var d = source.Value.ToString("yyyy-MM-dd 23:59:59");

        return DateTime.ParseExact(d, STANDARD_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Uses DateTime.TryParse(dateAsString, out var date)
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
    ///     Compare <paramref name="date1" /> with <paramref name="date2" />.
    ///     <inheritdoc cref="ToComparableDateTime(DateTime,int)" />
    /// </summary>
    public static bool IsEqualTo(this DateTime date1, DateTime date2, int decimalPlaces = 2)
    {
        var formattedDate1 = date1.ToComparableDateTime(decimalPlaces);
        var formattedDate2 = date2.ToComparableDateTime(decimalPlaces);
        var isEqual = formattedDate1 == formattedDate2;

        return isEqual;
    }

    /// <summary>
    ///     <inheritdoc cref="IsEqualTo(DateTime,DateTime,int)" />
    ///     <para>Will return true of both values are null.</para>
    /// </summary>
    public static bool IsEqualTo(this DateTime? date1, DateTime? date2, int decimalPlaces = 2)
    {
        if (date1 == null && date2 == null)
        {
            return true;
        }

        if (date1 == null || date2 == null)
        {
            return false;
        }

        var formattedDate1 = date1.ToComparableDateTime(decimalPlaces);
        var formattedDate2 = date2.ToComparableDateTime(decimalPlaces);
        var isEqual = formattedDate1 == formattedDate2;

        return isEqual;
    }

    /// <summary>
    ///     <para>Round off the milliseconds before comparing.</para>
    ///     <para>
    ///         This is a workaround because the 'FF' date format simply truncates the milliseconds and does not perform
    ///         rounding off.
    ///     </para>
    /// </summary>
    public static string ToComparableDateTime(this DateTime date1, int decimalPlaces = 2)
    {
        var ms = new string('F', decimalPlaces + 1);

        var ms1 = decimal.Parse($"0.{date1.ToString(ms)}") // truncate milliseconds to decimalPlaces + 1
            .ToString($"F{decimalPlaces}") // round off
            .Replace("0.", string.Empty); // remove decimal points

        var dt1 = date1.ToString(STANDARD_DATE_TIME_FORMAT);
        var formattedDate1 = $"{dt1}.{ms1}";

        return formattedDate1;
    }

    /// <summary>
    ///     Returns date in yyyy-MM-dd format.
    /// </summary>
    public static string ToDate(this DateTime date1)
    {
        return date1.ToString(STANDARD_DATE);
    }

    /// <summary>
    ///     <inheritdoc cref="ToDate(System.DateTime)" />
    /// </summary>
    public static string ToDate(this DateTime? date1)
    {
        if (date1.HasValue)
        {
            return date1.Value.ToString(STANDARD_DATE);
        }

        return string.Empty;
    }

    /// <summary>
    ///     <inheritdoc cref="ToComparableDateTime(DateTime,int)" />
    /// </summary>
    public static string ToComparableDateTime(this DateTime? date1, int decimalPlaces = 2)
    {
        if (date1 == null)
        {
            return string.Empty;
        }

        var formattedDate1 = date1.Value.ToComparableDateTime(decimalPlaces);

        return formattedDate1;
    }

    /// <summary>
    ///     If showMilliseconds is false, will round milliseconds to the nearest seconds.
    /// </summary>
    public static string GetDuration(this DateTime start, DateTime end, bool showMilliseconds = true)
    {
        var elapsedTime = end - start;

        if (showMilliseconds)
        {
            return $"{elapsedTime.Hours:00}:{elapsedTime.Minutes:00}:{elapsedTime.Seconds:00}.{elapsedTime.Milliseconds / 10:00}";
        }

        var roundedMsToSecValue = elapsedTime.Milliseconds / 1000;
        var seconds = elapsedTime.Seconds + roundedMsToSecValue;

        return $"{elapsedTime.Hours:00}:{elapsedTime.Minutes:00}:{seconds:00}";
    }

    /// <summary>
    ///     <inheritdoc cref="GetDuration(System.DateTime,System.DateTime,bool)" />
    /// </summary>
    public static string GetDuration(this DateTime? start, DateTime? end, DateTime defaultValue, bool showMilliseconds = true)
    {
        var start1 = start ?? defaultValue;
        var end2 = end ?? defaultValue;
        var duration = start1.GetDuration(end2, showMilliseconds);

        return duration;
    }

    /// <summary>
    ///     Will stop and reset the Stopwatch.
    /// </summary>
    public static string StopAndGetDuration(this Stopwatch stopwatch)
    {
        stopwatch.Stop();

        var elapsedTime = stopwatch.Elapsed;
        var duration = $"{elapsedTime.Hours:00}:{elapsedTime.Minutes:00}:{elapsedTime.Seconds:00}.{elapsedTime.Milliseconds / 10:00}";

        stopwatch.Reset();

        return duration;
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
        var callSite = stackTraceSplit?.FirstOrDefault(x => Regex.IsMatch(x, regExPattern))?.Trim() ?? string.Empty;
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
