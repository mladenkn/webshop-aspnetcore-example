using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.DataAccess
{
    public interface IQueries
    {
        Task<Basket> GetUsersBasketWithItemsAndDiscounts(string userId);
        Task<Basket> GetUsersBasket(string userId);
    }
}
