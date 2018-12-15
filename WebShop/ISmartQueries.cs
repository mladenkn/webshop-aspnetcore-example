using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop
{
    public interface ISmartQueries
    {
        Task<IReadOnlyCollection<Discount>> GetDiscountsFor(Basket basket);
    }
}
