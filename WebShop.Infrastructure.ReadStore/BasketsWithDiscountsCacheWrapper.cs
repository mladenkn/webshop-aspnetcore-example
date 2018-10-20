using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationKernel.Domain;
using Microsoft.EntityFrameworkCore;
using WebShop.BasketItems;
using WebShop.Baskets;
using WebShop.Discounts;

namespace WebShop.Infrastructure.PersistentCache
{
    public delegate void AddItemToBasket(BasketItem item);
    public delegate Task AddBasket(int basketId);

    public class BasketsWithDiscountsCacheWrapper
    {
        private readonly IJobQueue _jobs;
        private readonly IQueryable<Discount> _discountsTable;
        private readonly IQueryable<Basket> _basketTable;
        private readonly IBasketsWithDiscountsCache _cache;
        private readonly GetBasketWithDiscountsApplied _getBasket;
        private readonly AddDiscountsToBasketItem _addDiscountsToBasketItem;

        public BasketsWithDiscountsCacheWrapper(
            IJobQueue jobs, 
            IQueryable<Discount> discountsTable, 
            IQueryable<Basket> basketTable,
            IBasketsWithDiscountsCache cache,
            GetBasketWithDiscountsApplied getBasket,
            AddDiscountsToBasketItem addDiscountsToBasketItem)
        {
            _jobs = jobs;
            _discountsTable = discountsTable;
            _basketTable = basketTable;
            _cache = cache;
            _getBasket = getBasket;
            _addDiscountsToBasketItem = addDiscountsToBasketItem;
        }

        public async Task<Basket> GetBasketWithDiscountsApplied(int basketId)
        {
            var job = _jobs.Current.OfType<CacheBasketItemJob>().FirstOrDefault(j => j.BasketId == basketId);

            if (job == null)
                return await _cache.GetBasketWithDiscountsApplied(basketId);
            else
            {
                await job.Task;
                return await _cache.GetBasketWithDiscountsApplied(basketId);
            }
        }

        public async Task AddBasket(int basketId)
        {
            var basket = await _getBasket(basketId);
            await _cache.Add(basket);
        }

        public void AddItemToBasket(BasketItem item)
        {
            async Task AddActual()
            {
                var productDiscounts = await _discountsTable
                    .Where(d => d.TargetProductId == item.ProductId)
                    .ToArrayAsync();
                item.Basket = await _basketTable.FirstOrDefaultAsync(b => b.Id == item.BasketId);
                _addDiscountsToBasketItem(item, productDiscounts, new List<DiscountGranted>());
                await _cache.AddItem(item);
            }

            var job = new CacheBasketItemJob
            {
                BasketId = item.BasketId,
                BasketItemId = item.Id,
                Task = AddActual()
            };

            _jobs.Add(job);
        }

        private class CacheBasketItemJob : IJob
        {
            public Task Task { get; set; }
            public int BasketId { get; set; }
            public int BasketItemId { get; set; }
        }
    }
}
