using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eml.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// return mainList.Except(listToRemove, (x, y) => x.Id == y.Id);
        /// </summary>
        public static List<TSource> Except<TSource>(this IEnumerable<TSource> mainList,
            IEnumerable<TSource> listToRemove, Func<TSource, TSource, bool> comparer)
        {
            return mainList.Except(listToRemove, new LambdaEqualityComparer<TSource>(comparer)).ToList();
        }

        /// <summary>
        /// return mainList.Intersect(listToRemove, (x, y) => x.Id == y.Id);
        /// </summary>
        public static List<TSource> Intersect<TSource>(this IEnumerable<TSource> mainList,
            IEnumerable<TSource> listToMerge, Func<TSource, TSource, bool> comparer)
        {
            return mainList.Intersect(listToMerge, new LambdaEqualityComparer<TSource>(comparer)).ToList();
        }

        public static async Task ForEachAsync<T>(this IEnumerable<T> list, Func<T, Task> func)
        {
            foreach (var value in list)
            {
                await func(value);
            }
        }
    }
}