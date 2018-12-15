using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop
{
    public interface IQueries
    {
        Task<Basket> GetBasketWithItems(int basketId);
    }
}
