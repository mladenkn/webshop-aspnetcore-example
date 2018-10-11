using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Utilities;
using WebShop.Abstract;
using WebShop.Discounts;

namespace WebShop.Baskets
{
    public interface IBasketService
    {
        Task<Basket> GetBasket(int id);
        void CalculatePrice(Basket basket);
        void CalculatePrice(BasketItem item);
        Task<BasketItem> AddBasketItem(BasketItem basketItem);
        Task<IEnumerable<BasketItem>> GetBasketItemsDiscountableWith(Discount discount);
    }

    public class BasketService : IBasketService
    {
        private readonly IDiscountService _discountService;
        private readonly NewTransaction _newTransaction;
        private readonly IQueryable<BasketItem> _basketItems;
        private readonly IQueryable<Basket> _baskets;
        private readonly decimal _maxAllowedDiscount;

        public BasketService(
            IDiscountService discountService,
            NewTransaction newTransaction, 
            IQueryable<BasketItem> basketItems,
            IQueryable<Basket> baskets,
            decimal maxAllowedDiscount)
        {
            _discountService = discountService;
            _newTransaction = newTransaction;
            _basketItems = basketItems;
            _baskets = baskets;
            _maxAllowedDiscount = maxAllowedDiscount;
        }

        public async Task<Basket> GetBasket(int id)
        {
            var basket = await _baskets.FirstOrDefaultAsync(b => b.Id == id);
            CalculatePrice(basket);
            return basket;
        }

        public void CalculatePrice(Basket basket)
        {
            basket.Items.Must().NotBeNull();

            basket.TotalPrice = basket.Items.Select(i => i.Price).Sum();
        }

        public void CalculatePrice(BasketItem item)
        {
            item.Product.Must().NotBeNull();
            item.Discounts.Must().NotBeNull();

            var totalDiscount = item.Discounts.Select(d => d.Value).Sum();
            if (totalDiscount > _maxAllowedDiscount)
                totalDiscount = _maxAllowedDiscount;

            var without = item.Product.RegularPrice * totalDiscount;
            item.Price = item.Product.RegularPrice - without;
        }

        public async Task<BasketItem> AddBasketItem(BasketItem basketItem)
        {
            await _newTransaction().Save(basketItem).Commit();
            var discounts = await _discountService.GetDiscountsFor(basketItem);
            await discounts.Select(d => _discountService.Discount(basketItem, d)).WhenAll();
            return basketItem;
        }

        public async Task<IEnumerable<BasketItem>> GetBasketItemsDiscountableWith(Discount discount)
        {
            var basketItems = await _basketItems
                .Where(i => i.Product.Id == discount.ForProductId &&
                            i.Basket.Items.Count(bi => bi.ProductId == discount.ForProductId) >=
                            discount.RequiredMinimalQuantity)
                .Take(discount.MaxNumberOfItemsToApplyTo)
                .ToListAsync();
            return basketItems;
        }
    }
}
