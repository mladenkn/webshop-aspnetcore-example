using System.Collections.Generic;
using System.Linq;

namespace WebShop
{
    public static class Utilities
    {
        public static bool AnyDuplicates<T>(this IEnumerable<T> values)
        {
            var set = new HashSet<T>();
            return values.All(item => set.Add(item));
        }
    }
}
