using System;
using System.Threading.Tasks;
using WebShop.BasketItems;
using WebShop.Baskets;

namespace WebShop.Infrastructure.ReadStore
{
    public interface IBasketLowLevelRepository
    {
        Task<Basket> GetBasketWithDiscountsApplied(int basketId);
        Task AddItemToBasket(BasketItem item);
    }

    public class BasketMongoRepository : IBasketLowLevelRepository
    {
        public Task<Basket> GetBasketWithDiscountsApplied(int basketId)
        {
            throw new NotImplementedException();
        }

        public Task AddItemToBasket(BasketItem item)
        {
            throw new NotImplementedException();
        }
    }
}
