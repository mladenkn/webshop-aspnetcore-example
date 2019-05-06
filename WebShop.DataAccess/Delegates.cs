using System.Linq;

namespace WebShop.DataAccess
{
    public delegate IQueryable<T> Query<T>(IQueryable<T> queryable);
}
