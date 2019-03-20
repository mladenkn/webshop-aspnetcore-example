using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Infrastructure.DataAccess
{
    public delegate IQueryable<T> Query<T>(IQueryable<T> queryable);
}
