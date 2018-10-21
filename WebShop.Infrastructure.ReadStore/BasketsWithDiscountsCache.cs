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
    public delegate void AddItemToBasket(BasketItem item);
    public delegate Task AddBasket(int basketId);

    public class BasketsWithDiscountsCache
    {
        private readonly IJobQueue _jobs;
        private readonly IQueryable<Discount> _discountsTable;
        private readonly IQueryable<Basket> _basketTable;
        private readonly IBasketsWithDiscountsLowLevelCache _lowLevelCache;
        private readonly GetBasketWithDiscountsApplied _getBasket;
        private readonly AddDiscountsToBasketItem _addDiscountsToBasketItem;

        public BasketsWithDiscountsCache(
            IJobQueue jobs, 
            IQueryable<Discount> discountsTable, 
            IQueryable<Basket> basketTable,
            IBasketsWithDiscountsLowLevelCache lowLevelCache,
            GetBasketWithDiscountsApplied getBasket,
            AddDiscountsToBasketItem addDiscountsToBasketItem)
        {
            _jobs = jobs;
            _discountsTable = discountsTable;
            _basketTable = basketTable;
            _lowLevelCache = lowLevelCache;
            _getBasket = getBasket;
            _addDiscountsToBasketItem = addDiscountsToBasketItem;
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

        public async Task AddBasket(int basketId)
        {
            var basket = await _getBasket(basketId);
            await _lowLevelCache.Add(basket);
        }

        public void AddItemToBasket(BasketItem item)
        {
            async Task AddActual()
            {
                var productDiscounts = await _discountsTable
                    .Where(d => d.TargetProductId == item.ProductId)
                    .ToArrayAsync();

                item.Basket = await _basketTable
                    .Include(b => b.Items)
                    .FirstOrDefaultAsync(b => b.Id == item.BasketId);

                _addDiscountsToBasketItem(item, productDiscounts, new List<DiscountGranted>());
                await _lowLevelCache.AddItem(item);
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
