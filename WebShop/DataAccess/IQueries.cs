using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.DataAccess
{
    public interface IQueries
    {
        Task<Basket> GetBasketWithItems(int basketId);
    }
}
