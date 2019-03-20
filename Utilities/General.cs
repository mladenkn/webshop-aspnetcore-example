using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public static class General
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var x in enumerable)
                action(x);
        }
    }
}
