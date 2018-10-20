using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShop.BasketItems;
using WebShop.Baskets;
using WebShop.Discounts;

namespace WebShop.Infrastructure.ReadStore.Refreshing
{
    public interface IDataRefresher
    {
        void RefreshBasketWithItemJob(BasketItem item);
    }

    public class DataRefresher : IDataRefresher
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

        public void RefreshBasketWithItemJob(BasketItem item)
        {
            async Task RefreshActual()
            {
                var productDiscounts = await _discounts
                    .Where(d => d.TargetProductId == item.ProductId)
                    .ToArrayAsync();
                item.Basket = await _baskets.FirstOrDefaultAsync(b => b.Id == item.BasketId);
                _addDiscountsToBasketItem(item, productDiscounts, new List<DiscountGranted>());
                await _addToStore(item);
            }

            var job = new RefreshBasketWithItemJob
            {
                BasketId = item.BasketId,
                BasketItemId = item.Id,
                Task = RefreshActual()
            };

            _jobs.Add(job);
        }
    }
}
