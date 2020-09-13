using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

        public static IQueryable<TSource> Paginate<TSource>(this IQueryable<TSource> source, int pageNumber, int pageSize)
        {
            if (pageNumber == 1) return source.Take(pageSize);

            var resultsToSkip = (pageNumber - 1) * pageSize;

            return source
                .Skip(resultsToSkip)
                .Take(pageSize);
        }

        /// <summary>
        /// Sample nameSelector: r => r.Text
        /// Sample valueSelector: r => r.Value
        /// Will add dropdown default value: - Select - 
        /// Convert Lists into MVC-ish dropdown list. Call the ToMvcSelectList extenstion when using HTML.DropDownListFor.
        /// Set includeDefaultValue = false to exclude the default value:  '- Select -'
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

        /// <summary>
        /// Sample mergeExpression: Expression.And
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="mergeExpression"></param>
        /// <returns></returns>
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> mergeExpression)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(mergeExpression(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        public static Expression<Func<T, TResult>> ToExpression<T, TResult>(this Func<T, TResult> method)
        {
            return x => method(x);
        }
    }
}
