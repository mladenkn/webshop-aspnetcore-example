using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationKernel;
using Microsoft.EntityFrameworkCore;
using Utilities;
using WebShop.BasketItems;
using WebShop.Baskets;
using WebShop.Discounts;

namespace WebShop.Infrastructure.PersistentCache
{
    public interface IBasketsWithDiscountsCache
    {
        Task Add(int basketId);
        void AddItem(BasketItem item);
    }

    public class BasketsWithDiscountsCache : IBasketsWithDiscountsCache
    {
        private readonly IJobQueue _jobs;
        private readonly IBasketsWithDiscountsLowLevelCache _lowLevelCache;
        private readonly GetBasketWithDiscountsApplied _getBasket;

        public BasketsWithDiscountsCache(
            IJobQueue jobs, 
            IBasketsWithDiscountsLowLevelCache lowLevelCache,
            GetBasketWithDiscountsApplied getBasket)
        {
            _jobs = jobs;
            _lowLevelCache = lowLevelCache;
            _getBasket = getBasket;
        }

        public async Task<Basket> GetBasketWithDiscountsApplied(int basketId)
        {
            var basketJobs = _jobs.Jobs.OfType<IBasketCacheJob>().Where(j => j.BasketId == basketId);
            await basketJobs.Select(j => j.Task).WhenAll();
            return await _lowLevelCache.Get(basketId);
        }

        public async Task Add(int basketId)
        {
            var basket = await _lowLevelCache.Get(basketId);
            await _lowLevelCache.Add(basket);
        }

        public void AddItem(BasketItem item)
        {
            async Task AddActual()
            {
                // need to actually update whole basket because the total price has changed
                var basket = await _getBasket(item.BasketId); // there is probably a faster way
                await _lowLevelCache.Update(basket);
            }

            var job = new CacheBasketItemJob
            {
                BasketId = item.BasketId,
                BasketItemId = item.Id,
                Task = AddActual()
            };

            _jobs.Add(job);
        }
    }

    public class CacheBasketItemJob : IBasketCacheJob
    {
        public Task Task { get; set; }
        public int BasketId { get; set; }
        public int BasketItemId { get; set; }
    }
}
