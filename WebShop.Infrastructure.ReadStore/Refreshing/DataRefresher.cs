using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShop.BasketItems;
using WebShop.Baskets;
using WebShop.Discounts;
using WebShop.Infrastructure.ReadStore.Queries;

namespace WebShop.Infrastructure.ReadStore.Refreshing
{
    public delegate void RefreshBasketWithItem(BasketItem item);

    public class DataRefresher
    {
        private readonly IQueryable<Discount> _discounts;
        private readonly IQueryable<Basket> _baskets;
        private readonly AddItemToBasketStore _addToStore;
        private readonly AddDiscountsToBasketItem _addDiscountsToBasketItem;
        private readonly IDataRefreshJobsQueue _jobs;

        public DataRefresher(
            IQueryable<Discount> discounts,
            IQueryable<Basket> baskets,
            AddItemToBasketStore addToStore,
            AddDiscountsToBasketItem addDiscountsToBasketItem, 
            IDataRefreshJobsQueue jobs)
        {
            _discounts = discounts;
            _baskets = baskets;
            _addToStore = addToStore;
            _addDiscountsToBasketItem = addDiscountsToBasketItem;
            _jobs = jobs;
        }
        
        private async Task RefreshBasketWithItemJobActual(BasketItem item)
        {
            var productDiscounts = await _discounts
                .Where(d => d.TargetProductId == item.ProductId)
                .ToArrayAsync();
            item.Basket = await _baskets.FirstOrDefaultAsync(b => b.Id == item.BasketId);
            _addDiscountsToBasketItem(item, productDiscounts, new List<DiscountGranted>());
            await _addToStore(item);
        }

        public void RefreshBasketWithItemJob(BasketItem item)
        {
            var job = new RefreshBasketWithItemJob
            {
                BasketId = item.BasketId,
                BasketItemId = item.Id,
                Task = RefreshBasketWithItemJobActual(item)
            };
            _jobs.Add(job);
        }
    }
}
