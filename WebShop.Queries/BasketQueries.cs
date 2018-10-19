using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Utilities;
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

            var allDiscounts = await _discountStore.ToListAsync();

            var grantedDiscounts = new List<DiscountGranted>();
            foreach (var basketItem in basket.Items)
            {
                basketItem.Discounts = GetDiscountsFor(basketItem, allDiscounts, grantedDiscounts);
                CalculateItemPrice(basketItem);
            }

            basket.TotalPrice = basket.Items.Select(i => i.Price).Sum();

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

        private List<Discount> GetDiscountsFor(
            BasketItem basketItem, IEnumerable<Discount> allDiscounts, ICollection<DiscountGranted> grantedDiscounts)
        {
            bool ShouldDiscountWith(Discount discount)
            {
                if (discount.TargetProductId != basketItem.ProductId)
                    return false;

                var numOfTimesToGrant = 
                    basketItem.Basket.Items.Count(it => it.ProductId == discount.RequiredProductId) /
                    discount.RequiredProductQuantity;
                
                var isGrantedMaxTimes = grantedDiscounts
                    .ContainsN(it => it.ProductId == basketItem.ProductId && it.DiscountId == discount.Id, numOfTimesToGrant);

                var shouldGrant = !isGrantedMaxTimes;

                if (shouldGrant)
                    grantedDiscounts.Add(new DiscountGranted(basketItem.ProductId, discount.Id));

                return shouldGrant;
            }

            return allDiscounts
                .Where(ShouldDiscountWith)
                .ToList();
        }

        private struct DiscountGranted
        {
            public DiscountGranted(int productId, int discountId) : this()
            {
                ProductId = productId;
                DiscountId = discountId;
            }

            public int ProductId { get; }
            public int DiscountId { get; }
        }
    }
}
