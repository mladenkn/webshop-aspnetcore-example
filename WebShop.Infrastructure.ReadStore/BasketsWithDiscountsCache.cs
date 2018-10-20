using System;
using System.Threading.Tasks;
using WebShop.BasketItems;
using WebShop.Baskets;

namespace WebShop.Infrastructure.PersistentCache
{
    public interface IBasketsWithDiscountsCache
    {
        Task<Basket> GetBasketWithDiscountsApplied(int basketId);
        Task AddItem(BasketItem item);
        Task Add(Basket basket);
    }

    public class BasketsWithDiscountsMongoRepository : IBasketsWithDiscountsCache
    {
        public Task<Basket> GetBasketWithDiscountsApplied(int basketId)
        {
            throw new NotImplementedException();
        }

        public Task AddItem(BasketItem item)
        {
            throw new NotImplementedException();
        }

        public Task Add(Basket basket)
        {
            throw new NotImplementedException();
        }
    }
}
