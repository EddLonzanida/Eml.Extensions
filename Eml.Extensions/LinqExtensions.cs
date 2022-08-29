namespace Eml.Extensions;

public static class LinqExtensions
{
    /// <summary>
    ///     Remove items from <paramref name="mainList"></paramref>.
    ///     <para>Example:</para>
    ///     <code language="c#">return mainList.Except(listToRemove, (x, y) => x.Id == y.Id);</code>
    /// </summary>
    public static List<TSource> Except<TSource>(this IEnumerable<TSource> mainList,
        IEnumerable<TSource> listToRemove,
        Func<TSource?, TSource?, bool> comparer)
    {
        return mainList.Except(listToRemove, new LambdaEqualityComparer<TSource>(comparer)).ToList();
    }

    /// <summary>
    ///     Returns the intersection of 2 lists.
    ///     <para>Example:</para>
    ///     <code language="c#">return mainList.Intersect(listToRemove, (x, y) => x.Id == y.Id);</code>
    /// </summary>
    public static List<TSource> Intersect<TSource>(this IEnumerable<TSource> mainList,
        IEnumerable<TSource> listToIntersect,
        Func<TSource?, TSource?, bool> comparer)
    {
        return mainList.Intersect(listToIntersect, new LambdaEqualityComparer<TSource>(comparer)).ToList();
    }

    /// <summary>
    ///     Convert Lists into MVC-ish dropdown list. Call the ToMvcSelectList extenstion when using HTML.DropDownListFor.
    ///     Will add dropdown default value: - Select -
    ///     Set includeDefaultValue = false to exclude the default value:  '- Select -'
    ///     <para>Sample nameSelector: r => r.Text</para>
    ///     <para>Sample valueSelector: r => r.Value</para>
    /// </summary>
    public static IEnumerable<SelectListItem> ToSelectListItems<T, TName, TValue>(this List<T> items,
        Func<T, TValue> valueSelector,
        Func<T, TName> nameSelector,
        bool includeDefaultValue = true)
        where T : class
    {
        var tmpList = items
            .OrderBy(nameSelector)
            .ToList();

        var selectListItems = tmpList.Select(item => new SelectListItem
        {
            Text = nameSelector(item)?.ToString() ?? string.Empty,
            Value = valueSelector(item)?.ToString() ?? string.Empty //valueSelector
        }).ToList();

        if (includeDefaultValue)
        {
            selectListItems.Insert(0, new SelectListItem { Text = "- Select - ", Value = "" });
        }

        return selectListItems;
    }

    /// <summary>
    ///     Sample valueSelector: r => r.Value
    ///     Get the value in a List. Throws an Exception when multiple records found.
    /// </summary>
    public static TValue? GetValueAsync<T, TValue>(this List<T> items,
        Func<T, bool> idWhereClause,
        Func<T, TValue> valueSelector)
        where T : class
    {
        var results = items
            .Where(idWhereClause)
            .Select(valueSelector)
            .ToList();

        if (results.Count > 1)
        {
            throw new Exception($"{results.Count:G} records found. Expected 1.");
        }

        var firstOrDefault = results.FirstOrDefault();

        return firstOrDefault;
    }

    /// <summary>
    ///     Safely performs while loop. Used when deleting items from a mutable list.
    ///     <para>Uses <paramref name="items" />.Count to break the loop.</para>
    /// </summary>
    public static void While<T>(this List<T> items, Action<T> action)
    {
        var initialCount = items.Count;
        var ctr = initialCount - 1;

        if (initialCount <= 0)
        {
            return;
        }

        while (items.Count > 0 && ctr <= initialCount)
        {
            var item = items[0];

            action(item);

            ctr++;
        }
    }

    /// <summary>
    ///     <inheritdoc cref="While{T}" />
    /// </summary>
    public static async Task WhileAsync<T>(this List<T> items, Func<T, Task> action)
    {
        var initialCount = items.Count;
        var ctr = initialCount - 1;

        if (initialCount <= 0)
        {
            return;
        }

        while (items.Count > 0 && ctr <= initialCount)
        {
            var item = items[0];

            await action(item);

            ctr++;
        }
    }

    /// <summary>
    ///     <inheritdoc cref="GetDuplicateItemsLastInWins{T,TKey}(IEnumerable{T},Func{T,TKey},bool)" />
    /// </summary>
    public static List<T> GetDuplicateItemsLastInWins<T, TKey>(this IEnumerable<T> items, Func<T, bool> where, Func<T, TKey> keySelector, bool includeGroupCountIs1)
    {
        return items.GetUniqueItems(keySelector, where, includeGroupCountIs1);
    }

    /// <summary>
    ///     <para>Returns distinct list of <typeparamref name="T" />.</para>
    ///     <para>Get last row of the group. Last in wins.</para>
    /// </summary>
    public static List<T> GetDuplicateItemsLastInWins<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keySelector, bool includeGroupCountIs1)
    {
        return items.GetUniqueItems(keySelector, null, includeGroupCountIs1);
    }

    /// <summary>
    ///     <para>Returns distinct list of <typeparamref name="T" />.</para>
    ///     <para>Get last row of the group. First in wins.</para>
    /// </summary>
    public static List<T> GetDuplicateItemsFirstInWins<T, TKey>(this IEnumerable<T> items, Func<T, bool> where, Func<T, TKey> keySelector, bool includeGroupCountIs1)
    {
        return items.GetUniqueItems(keySelector, where, true, includeGroupCountIs1);
    }

    public static List<T> GetUniqueItems<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, bool>? where = null, bool includeGroupCountIs1 = false, bool lastInWins = true)
    {
        if (lastInWins)
        {
            if (includeGroupCountIs1)
            {
                return items
                    .Where(x => where?.Invoke(x) ?? true)
                    .GroupBy(keySelector)
                    .SelectMany(x => x.Skip(x.Count() - 1)) // Get first row of the group. last in wins.
                    .ToList();
            }

            return items
                .Where(x => where?.Invoke(x) ?? true)
                .GroupBy(keySelector)
                .Where(x => x.Count() > 1)
                .SelectMany(x => x.Skip(x.Count() - 1)) // Get first row of the group. last in wins.
                .ToList();
        }

        if (includeGroupCountIs1)
        {
            return items
                .Where(x => where?.Invoke(x) ?? true)
                .GroupBy(keySelector)
                .SelectMany(x => x.Take(1)) // Get first row of the group. first in wins.
                .ToList();
        }

        return items
            .Where(x => where?.Invoke(x) ?? true)
            .GroupBy(keySelector)
            .Where(x => x.Count() > 1)
            .SelectMany(x => x.Take(1)) // Get first row of the group. first in wins.
            .ToList();
    }
}
