using System;
using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<T> GenerateSequence<T>(Func<T> next, int count)
        {
            var r = new List<T>();
            for (var i = 0; i < count; i++)
                r.Add(next());
            return r;
        }

        public static IEnumerable<object> ConcatAll(params IEnumerable<object>[] enumerables) =>
            enumerables.SelectMany(o => o);
    }
}
