using System;
using System.Collections.Generic;
using System.Linq;
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
        public static void ForEach<T>(this IEnumerable<T> values, Func<T, object> action)
        {
            foreach (var value in values)
                action(value);
        }

        public static decimal PercentOf(this decimal percentage, decimal valueToBeDiscounted)
        {
            var dec = percentage / 100;
            var without = valueToBeDiscounted * dec;
            return valueToBeDiscounted - without;
        }

        public static decimal PercentOf(this int percentage, decimal valueToBeDiscounted)
        {
            return PercentOf((decimal)percentage, valueToBeDiscounted);
        }

        public static Task WhenAll(this IEnumerable<Task> tasks) => Task.WhenAll(tasks);

        public static MustAssertions Must<T>(this T o) => new MustAssertions(o);

        public static void SaveTo<T>(this T o, out T v)
        {
            v = o;
        }
    }

    public static class Collections
    {
        public static bool ContainsN<T>(this IEnumerable<T> enumerable, Func<T, bool> func, int count)
        {
            return enumerable.Count(func) == count;
        }

        public static bool ContainsOne<T>(this IEnumerable<T> enumerable, Func<T, bool> func)
        {
            return enumerable.ContainsN(func, 1);
        }

        public static IEnumerable<T> New<T>(Func<T> get, int count)
        {
            return Enumerable.Range(0, count)
                .Select(it => get());
        }

        public static IEnumerable<T> Concat<T>(params IEnumerable<T>[] collections)
        {
            return collections.Skip(1).Aggregate(collections.First(), (result, element) => result.Concat(element));
        }
    }
}
