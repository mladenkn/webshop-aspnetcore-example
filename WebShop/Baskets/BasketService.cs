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
    public class BasketService
    {
        private readonly IQueryable<Basket> _basketStore;
        private readonly IQueryable<Discount> _discountStore;
        private readonly AddDiscountsToBasketItem _addDiscountsToBasketItem;

        public BasketService(
            IQueryable<Basket> basketStore, 
            IQueryable<Discount> discountStore, 
            AddDiscountsToBasketItem addDiscountsToBasketItem)
        {
            _basketStore = basketStore;
            _discountStore = discountStore;
            _addDiscountsToBasketItem = addDiscountsToBasketItem;
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
                var discounts = allDiscounts.Where(d => d.TargetProductId == basketItem.ProductId);
                _addDiscountsToBasketItem(basketItem, discounts, grantedDiscounts);
            }

            basket.TotalPrice = basket.Items.Select(i => i.Price).Sum();

            return basket;
        }
    }
}
