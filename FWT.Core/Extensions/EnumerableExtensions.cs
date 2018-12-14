using System;
using System.Collections.Generic;

namespace FWTL.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static void AddWhenNotNull<T>(this ICollection<T> list, T item)
        {
            if (item != null)
            {
                list.Add(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        public static IEnumerable<TResult> ForEach<TModel, TResult>(this IEnumerable<TModel> enumeration, Func<TModel, TResult> action)
        {
            foreach (TModel item in enumeration)
            {
                yield return action(item);
            }
        }
    }
}
