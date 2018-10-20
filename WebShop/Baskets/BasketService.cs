using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ApplicationKernel.Domain.MediatorSystem;
using Microsoft.EntityFrameworkCore;
using Utilities;
using WebShop.BasketItems;
using WebShop.Discounts;

namespace WebShop.Baskets
{
    public delegate Task<Basket> GetBasketWithDiscountsApplied(int basketId);

    public class BasketService
    {
        private readonly IQueryable<Basket> _basketStore;
        private readonly IQueryable<Discount> _discountStore;
        private readonly ShouldApplyToBasketItem _shouldApplyToBasketItem;
        private readonly CalculateBasketItemPrice _calculateBasketItemPrice;

        public BasketService(
            IQueryable<Basket> basketStore, 
            IQueryable<Discount> discountStore, 
            ShouldApplyToBasketItem shouldApplyToBasketItem,
            CalculateBasketItemPrice calculateBasketItemPrice)
        {
            _basketStore = basketStore;
            _discountStore = discountStore;
            _shouldApplyToBasketItem = shouldApplyToBasketItem;
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
                basketItem.Discounts = allDiscounts.Where(d => _shouldApplyToBasketItem(basketItem, d, grantedDiscounts)).ToList();
                _calculateBasketItemPrice(basketItem);
            }

            basket.TotalPrice = basket.Items.Select(i => i.Price).Sum();

            return basket;
        }
    }
}
