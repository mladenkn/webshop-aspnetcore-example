using System;
using System.Threading.Tasks;
using WebShop.BasketItems;
using WebShop.Baskets;

namespace WebShop.Infrastructure.ReadStore
{
    public delegate Task AddItemToBasketStore(BasketItem item);

    public interface IReadStore
    {
        Task<Basket> GetBasket(int basketId);
    }

    public class MongoStore : IReadStore
    {
        public Task<Basket> GetBasket(int basketId)
        {
            throw new NotImplementedException();
        }

        public Task AddItemToBasketStore(BasketItem item)
        {
            throw new NotImplementedException();
        }
    }
}
