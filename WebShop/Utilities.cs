using System;
using System.Collections.Generic;

namespace WebShop
{
    public static class Utilities
    {
        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            foreach (var value in values)
                action(value);
        }
    }
}
