using System;
using System.Collections.Generic;
using System.Linq;

namespace Eml.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Remove items from <paramref name="mainList"></paramref>.
        /// <para>Example:</para>
        /// <code language="c#">return mainList.Except(listToRemove, (x, y) => x.Id == y.Id);</code>
        /// </summary>
        public static List<TSource> Except<TSource>(this IEnumerable<TSource> mainList,
            IEnumerable<TSource> listToRemove, Func<TSource, TSource, bool> comparer)
        {
            return mainList.Except(listToRemove, new LambdaEqualityComparer<TSource>(comparer)).ToList();
        }

        /// <summary>
        /// Returns the intersection of 2 lists.
        /// <para>Example:</para>
        /// <code language="c#">return mainList.Intersect(listToRemove, (x, y) => x.Id == y.Id);</code>
        /// </summary>
        public static List<TSource> Intersect<TSource>(this IEnumerable<TSource> mainList,
            IEnumerable<TSource> listToIntersect, Func<TSource, TSource, bool> comparer)
        {
            return mainList.Intersect(listToIntersect, new LambdaEqualityComparer<TSource>(comparer)).ToList();
        }

        /// <summary>
        /// Credit: <see href="https://github.com/morelinq/MoreLINQ/blob/master/MoreLinq/DistinctBy.cs">MoreLinq</see>
        /// </summary>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector, null);
        }

        /// <summary>
        /// Credit: <see href="https://github.com/morelinq/MoreLINQ/blob/master/MoreLinq/DistinctBy.cs">MoreLinq</see>
        /// </summary>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            return _(); IEnumerable<TSource> _()
            {
                var knownKeys = new HashSet<TKey>(comparer);
                foreach (var element in source)
                {
                    if (knownKeys.Add(keySelector(element)))
                        yield return element;
                }
            }
        }

        /// <summary>
        /// Convert Lists into MVC-ish dropdown list. Call the ToMvcSelectList extenstion when using HTML.DropDownListFor.
        /// Will add dropdown default value: - Select - 
        /// Set includeDefaultValue = false to exclude the default value:  '- Select -'
        /// <para>Sample nameSelector: r => r.Text</para>
        /// <para>Sample valueSelector: r => r.Value</para>
        /// </summary>
        public static IEnumerable<SelectListItem> ToSelectListItems<T, TName, TValue>(this List<T> items,
            Func<T, TValue> valueSelector, Func<T, TName> nameSelector, bool includeDefaultValue = true)
            where T : class
        {
            var tmpList = items
                .OrderBy(nameSelector)
                .ToList();

            var selectListItems = tmpList.Select(item => new SelectListItem
            {
                Text = nameSelector(item).ToString(),
                Value = valueSelector(item).ToString()   //valueSelector
            }).ToList();

            if (includeDefaultValue)
            {
                selectListItems.Insert(0, new SelectListItem { Text = "- Select - ", Value = "" });
            }

            return selectListItems;
        }

        /// <summary>
        /// Sample valueSelector: r => r.Value
        /// Get the value in a List. Throws an Exception when multiple records found.
        /// </summary>
        public static TValue GetValueAsync<T, TValue>(this List<T> items,
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
    }
}
