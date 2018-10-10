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
    public class BasketService : IBasketService
    {
        private readonly IDiscountService _discountService;
        private readonly NewTransaction _newTransaction;

        public BasketService(IDiscountService discountService, NewTransaction newTransaction)
        {
            _discountService = discountService;
            _newTransaction = newTransaction;
        }

        public void CalculatePrice(Basket basket)
        {
            basket.TotalPrice = basket.Items.Select(i => i.Price).Sum();
        }

        public void CalculatePrice(BasketItem item)
        {
            var without = item.Product.RegularPrice * item.Discount.Value;
            item.Price = item.Product.RegularPrice - without;
        }

        public async Task AddBasketItem(BasketItem basketItem)
        {
            await _newTransaction().Save(basketItem).Commit();
            var discounts = await _discountService.GetDiscountsFor(basketItem);
            await discounts.Select(d => _discountService.Discount(basketItem, d)).WhenAll();
        }

        public Task<IEnumerable<BasketItem>> GetBasketItemsDiscountableWith(Discount discount)
        {
            throw new NotImplementedException();
        }
    }

    public interface IBasketService
    {
        void CalculatePrice(Basket basket);
        void CalculatePrice(BasketItem item);
        Task AddBasketItem(BasketItem basketItem);
        Task<IEnumerable<BasketItem>> GetBasketItemsDiscountableWith(Discount discount);
    }
}
