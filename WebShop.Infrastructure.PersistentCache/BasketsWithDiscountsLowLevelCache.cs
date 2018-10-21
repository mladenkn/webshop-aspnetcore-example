using System.Threading.Tasks;
using WebShop.BasketItems;
using WebShop.Baskets;

namespace WebShop.Infrastructure.PersistentCache
{
    // The actual cache on disk, perhaps MongoDB collection
    public interface IBasketsWithDiscountsLowLevelCache
    {
        Task<Basket> GetBasketWithDiscountsApplied(int basketId);
        Task Add(Basket basket);
        Task Update(Basket basket);
    }
}
