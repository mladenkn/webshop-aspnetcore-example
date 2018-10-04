using System;
using System.Collections.Generic;
using System.Linq;

namespace WebShop.Domain
{
    public static class Utilities
    {
        public static bool AnyDuplicates<T>(this IEnumerable<T> values)
        {
            var set = new HashSet<T>();
            return values.All(item => set.Add(item));
        }

        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            foreach (var value in values)
                action(value);
        }
    }
}
