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
        private readonly IQueryable<Discount> _discountsTable;
        private readonly IQueryable<Basket> _basketTable;
        private readonly IBasketsWithDiscountsLowLevelCache _lowLevelCache;
        private readonly GetBasketWithDiscountsApplied _getBasket;

        public BasketsWithDiscountsCache(
            IJobQueue jobs, 
            IQueryable<Discount> discountsTable, 
            IQueryable<Basket> basketTable,
            IBasketsWithDiscountsLowLevelCache lowLevelCache,
            GetBasketWithDiscountsApplied getBasket)
        {
            _jobs = jobs;
            _discountsTable = discountsTable;
            _basketTable = basketTable;
            _lowLevelCache = lowLevelCache;
            _getBasket = getBasket;
        }

        public async Task<Basket> GetBasketWithDiscountsApplied(int basketId)
        {
            var basketJobs = _jobs.Jobs.OfType<IBasketCacheJob>().Where(j => j.BasketId == basketId).ToArray();

            if (basketJobs.Any())
            {
                await basketJobs.Select(j => j.Task).WhenAll();
                return await _lowLevelCache.GetBasketWithDiscountsApplied(basketId);
            }
            else
                return await _lowLevelCache.GetBasketWithDiscountsApplied(basketId);
        }

        public async Task Add(int basketId)
        {
            var basket = await _getBasket(basketId);
            await _lowLevelCache.Add(basket);
        }

        public void AddItem(BasketItem item)
        {
            async Task AddActual()
            {
                // need to actually update whole basket because the total price has changed
                var basket = await _getBasket(item.BasketId);
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
