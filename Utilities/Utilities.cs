using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class Collections
    {
        public static MustAssertions Must<T>(this T o) => new MustAssertions(o);
    }
}
