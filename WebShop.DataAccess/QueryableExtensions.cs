using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebShop.DataAccess
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Include<T>(
            this IQueryable<T> queryable, Action<IncludesBuilder<T>> include) where T : class
        {
            var includesBuilder = new IncludesBuilder<T>();
            include(includesBuilder);

            foreach (var propToInclude in includesBuilder.Includes)
                queryable = queryable.Include(propToInclude);

            return queryable;
        }
    }
}
