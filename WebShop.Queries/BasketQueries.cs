using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShop.Baskets;
using WebShop.Discounts;

namespace WebShop.Queries
{
    public class BasketQueries
    {
        private readonly IQueryable<Basket> _basketStore;
        private readonly IQueryable<Discount> _discountStore;
        private readonly decimal _maxAllowedDiscount;

        public BasketQueries(IQueryable<Basket> basketStore, IQueryable<Discount> discountStore, decimal maxAllowedDiscount)
        {
            _basketStore = basketStore;
            _discountStore = discountStore;
            _maxAllowedDiscount = maxAllowedDiscount;
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

            basket.TotalPrice = basket.Items.Select(i => i.Price).Sum();

            foreach (var basketItem in basket.Items)
            {
                basketItem.Discounts = await GetDiscountsFor(basketItem);
                CalculateItemPrice(basketItem);
            }

            return basket;
        }

        private void CalculateItemPrice(BasketItem item)
        {
            var totalDiscount = item.Discounts.Select(d => d.Value).Sum();
            if (totalDiscount > _maxAllowedDiscount)
                totalDiscount = _maxAllowedDiscount;

            var without = item.Product.RegularPrice * totalDiscount;
            item.Price = item.Product.RegularPrice - without;
        }

        private Task<List<Discount>> GetDiscountsFor(BasketItem item)
        {
            var numberOfProductsInBasket = item.Basket.Items.Count(i => i.ProductId == item.ProductId);
            return _discountStore
                .Where(d => d.ForProductId == item.ProductId &&
                            numberOfProductsInBasket >= d.RequiredMinimalQuantity)
                .ToListAsync();
        }
    }
}
