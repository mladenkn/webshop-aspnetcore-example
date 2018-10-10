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
        void CalculatePrice(Basket basket);
        void CalculatePrice(BasketItem item);
        Task AddBasketItem(BasketItem basketItem);
        Task<IEnumerable<BasketItem>> GetBasketItemsDiscountableWith(Discount discount);
    }

    public class BasketService : IBasketService
    {
        private readonly IDiscountService _discountService;
        private readonly NewTransaction _newTransaction;
        private readonly IQueryable<BasketItem> _basketItems;

        public BasketService(
            IDiscountService discountService,
            NewTransaction newTransaction, 
            IQueryable<BasketItem> basketItems)
        {
            _discountService = discountService;
            _newTransaction = newTransaction;
            _basketItems = basketItems;
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
            if (totalDiscount > 100)
                totalDiscount = 100;

            var without = item.Product.RegularPrice * totalDiscount;
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
}
