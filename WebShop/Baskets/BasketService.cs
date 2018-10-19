using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationKernel;
using Microsoft.EntityFrameworkCore;
using WebShop.BasketItems;
using WebShop.Discounts;

namespace WebShop.Baskets
{
    public delegate Task<Basket> GetBasketWithDiscountsApplied(int basketId);

    public class BasketService
    {
        private readonly IQueryable<Basket> _basketStore;
        private readonly IQueryable<Discount> _discountStore;
        private readonly GetDiscountsFor _getDiscountsFor;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly CalculateBasketItemPrice _calculateBasketItemPrice;

        public BasketService(
            IQueryable<Basket> basketStore, 
            IQueryable<Discount> discountStore, 
            GetDiscountsFor getDiscountsFor,
            IEventDispatcher eventDispatcher,
            CalculateBasketItemPrice calculateBasketItemPrice)
        {
            _basketStore = basketStore;
            _discountStore = discountStore;
            _getDiscountsFor = getDiscountsFor;
            _eventDispatcher = eventDispatcher;
            _calculateBasketItemPrice = calculateBasketItemPrice;
        }

        public async Task<Basket> GetBasketWithDiscountsApplied(int basketId)
        {
            var basket = await _basketStore
                .Where(b => b.Id == basketId)
                .Include(b => b.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync();

            if (basket == null)
                return null;

            var allDiscounts = await _discountStore.ToListAsync();

            var grantedDiscounts = new List<DiscountGranted>();
            foreach (var basketItem in basket.Items)
            {
                basketItem.Discounts = _getDiscountsFor(basketItem, allDiscounts, grantedDiscounts);
                _calculateBasketItemPrice(basketItem);
            }

            basket.TotalPrice = basket.Items.Select(i => i.Price).Sum();

            await _eventDispatcher.Dispatch(new BasketSumCalculatedEvent(basket));

            return basket;
        }
    }
}
