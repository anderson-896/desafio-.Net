using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Extensions
{
    public static class IEnumerableExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection, Func<T, bool> predicate = null)
        {
            if (predicate == null)
                return collection == null || !collection.Any();
            else
                return collection == null || !collection.Any(predicate);
        }
    }
}
