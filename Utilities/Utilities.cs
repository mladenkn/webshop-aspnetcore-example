using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Utils
    {
        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            foreach (var value in values)
                action(value);
        }

        public static Task WhenAll(this IEnumerable<Task> tasks) => Task.WhenAll(tasks);

        public static MustAssertions Must<T>(this T o) => new MustAssertions(o);
    }
}
