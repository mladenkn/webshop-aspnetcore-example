using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.Services
{
    public interface ICurrentUserProvider
    {
        string GetId();
    }

    public interface IBasketWithPriceCache
    {
        Task<BasketWithPrice> Get(int basketId);
        Task Set(BasketWithPrice b);
        void Invalidate(int basketId);
        bool IsValid(int basketId);
    }
}
