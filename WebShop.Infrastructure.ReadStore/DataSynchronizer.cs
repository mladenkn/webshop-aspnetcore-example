using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShop.BasketItems;
using WebShop.Baskets;
using WebShop.Discounts;

namespace WebShop.Infrastructure.ReadStore
{
    public delegate void AddItemToBasket(BasketItem item);

    public class DataSynchronizer
    {
        private readonly IQueryable<Discount> _discounts;
        private readonly IQueryable<Basket> _baskets;
        private readonly AddItemToBasketStore _addToStore;
        private readonly AddDiscountsToBasketItem _addDiscountsToBasketItem;
        private readonly IDataSyncJobsQueue _jobs;

        public DataSynchronizer(
            IQueryable<Discount> discounts,
            IQueryable<Basket> baskets,
            AddItemToBasketStore addToStore,
            AddDiscountsToBasketItem addDiscountsToBasketItem, 
            IDataSyncJobsQueue jobs)
        {
            _discounts = discounts;
            _baskets = baskets;
            _addToStore = addToStore;
            _addDiscountsToBasketItem = addDiscountsToBasketItem;
            _jobs = jobs;
        }
        
        private async Task AddItemToBasketActual(BasketItem item)
        {
            var productDiscounts = await _discounts
                .Where(d => d.TargetProductId == item.ProductId)
                .ToArrayAsync();
            item.Basket = await _baskets.FirstOrDefaultAsync(b => b.Id == item.BasketId);
            _addDiscountsToBasketItem(item, productDiscounts, new List<DiscountGranted>());
            await _addToStore(item);
        }

        public void AddItemToBasket(BasketItem item)
        {
            var job = new AddItemToBasketJob
            {
                BasketId = item.BasketId,
                BasketItemId = item.Id,
                Task = AddItemToBasketActual(item)
            };
            _jobs.Add(job);
        }
    }
}
