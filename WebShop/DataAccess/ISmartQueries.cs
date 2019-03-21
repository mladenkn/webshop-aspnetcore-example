using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.DataAccess
{
    public interface ISmartQueries
    {
        Task<IEnumerable<Discount>> GetDiscountsFor(Basket basket);
    }
}
